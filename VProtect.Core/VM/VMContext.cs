using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using VProtect.Core.Runtime;
using VProtect.Core.Utilites;
using VProtect.Core.Services;
using VProtect.Core.VM.InjRuntime;
using VProtect.Core.Services.Injection;

using DOpCodes = dnlib.DotNet.Emit.OpCodes;

namespace VProtect.Core.VM
{
    public class VMContext: IDisposable
    {
        private MethodReadWrite MDreadWrite;

        public byte[] GetRawModuleBytes
        {
            get;
            private set;
        }

        public ModuleDefMD Module
        {
            get;
            private set;
        }

        public ModuleDefMD RTModule
        {
            get;
            private set;
        }

        internal ModuleWriterBase ModuleWriter
        {
            get;
            private set;
        }

        internal MethodDef ModuleStaticConstractor
        {
            get;
            private set;
        }

        internal RuntimeSearch RTSearch
        {
            get;
            private set;
        }

        internal RTConstants RTConstants
        {
            get;
            private set;
        }

        internal MethodPatcher MethodPatcher
        {
            get;
            private set;
        }

        internal RTRenamer RTRenamer
        {
            get;
            private set;
        }

        internal List<MethodDef> MethodList
        {
            get;
            private set;
        }

        internal MethodDef SelectedMethod
        {
            get;
            private set;
        }

        internal LocalList SelectedMethodLocals
        {
            get;
            private set;
        }

        internal List<VInstruction> Instructions
        {
            get;
            private set;
        }

        public VMContext(ModuleDefMD module, ModuleDefMD rtModule)
        {
            GetRawModuleBytes = File.ReadAllBytes(module.Location);
            Module = module;
            RTModule = rtModule;
            MethodList = new List<MethodDef>();

            RTSearch = new RuntimeSearch(this);
            RTConstants = new RTConstants(this);
            MethodPatcher = new MethodPatcher(this);

            MDreadWrite = new MethodReadWrite(this);
            RTRenamer = new RTRenamer(this);
        }

        internal void SetSettings(List<MethodDef> mdList, MethodDef cctor)
        {
            // Set Module .cctor
            ModuleStaticConstractor = cctor;

            // Set Method List
            for (int i = 0; i < mdList.Count; i++)
            {
                var method = mdList[i];

                if (!method.HasBody)
                    continue;

                if (method.HasGenericParameters)
                    continue;

                if (method.IsPinvokeImpl)
                    continue;

                if (method.IsUnmanagedExport)
                    continue;

                MethodList.Add(method);
            }

            // Rename VProtect.Runtime
            //RTRenamer.Process();

            // Clear Attributes
            RTModule.Assembly.CustomAttributes.Clear();
        }

        internal void SetMethod(ModuleWriterBase writer, MethodDef method)
        {
            ModuleWriter = writer;
            SelectedMethod = method;
            SelectedMethodLocals = method.Body.Variables;
        }

        internal void Patch()
        {
            Instructions = new List<VInstruction>();

            MDreadWrite.Read();
            MDreadWrite.Write();
        }

        internal void InjectMergeRuntime()
        {
            var typeDef = ModuleDefMD.Load(typeof(Merge).Module).ResolveTypeDef(MDToken.ToRID(typeof(Merge).MetadataToken));
            var members = InjectHelper.Inject(typeDef, Module.GlobalType, Module);

            var methods = new HashSet<MethodDef>
            {
                members.OfType<MethodDef>().Single(method => method.Name == "cctor"),
                members.OfType<MethodDef>().Single(method => method.Name == "CurrentDomain_AssemblyResolve"),
                members.OfType<MethodDef>().Single(method => method.Name == "GetFirstHeapStreamMapped"),
                members.OfType<MethodDef>().Single(method => method.Name == "GetFirstHeapStreamFlat")
            };

            #region Rename Methods
            ///////////////////////////////////////////////////////////////////////
            foreach (IDnlibDef def in members)
            {
                IMemberDef memberDef = def as IMemberDef;

                if ((memberDef as MethodDef) != null)
                    memberDef.Name = RTRenamer.NewName(memberDef.Name);
                else if ((memberDef as FieldDef) != null)
                    memberDef.Name = RTRenamer.NewName(memberDef.Name);
            }
            ///////////////////////////////////////////////////////////////////////
            #endregion

            var ctor = Module.GlobalType.FindOrCreateStaticConstructor();
            for (int i = 0; i < (methods.ToArray()[0].Body.Instructions.Count - 1); i++)
                ctor.Body.Instructions.Insert(i, methods.ToArray()[0].Body.Instructions[i]);

            Module.GlobalType.Methods.Remove(methods.ToArray()[0]);
        }

        internal void Final()
        {
            var libBuff = MDreadWrite.Final();
            MDreadWrite.Dispose();

            // Add Lib Buffer Heap
            ModuleWriter.TheOptions.MetadataOptions.MetadataHeapsAdded += delegate (object sender, MetadataHeapsAddedEventArgs e)
            {
                e.Heaps.Insert(0, new RawHeap($"#{DataDescriptor.RandomGenerator.NextHexString(8, true)}", QuickLZ.Compress(libBuff.ToArray())));
            };
        }

        public void Dispose()
        {
            RTSearch?.Dispose();

            SelectedMethod = null;

            Module = null;
            RTModule?.Dispose();
            ModuleWriter = null;

            SelectedMethodLocals?.Clear();
        }
    }
}
