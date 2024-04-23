using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using VProtect.Core.VM;
using VProtect.Core.OpCodes;

using MethodAttributes = dnlib.DotNet.MethodAttributes;

namespace VProtect.Core
{
    public class VMRun
    {
        public VMContext GetVMContext
        {
            get;
            private set;
        }

        public ModuleWriterOptions Options
        {
            get;
            private set;
        }

        public VMRun(ModuleDefMD module)
        {
            GetVMContext = new VMContext(module, ModuleDefMD.Load(typeof(VProtect.Runtime.Entry).Module));
        }

        public void Run(List<MethodDef> methods)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var oldType = GetVMContext.Module.GlobalType;
            var newType = new TypeDefUser(oldType.Name);

            oldType.Name = "<VProtect>";
            oldType.BaseType = GetVMContext.Module.CorLibTypes.GetTypeRef("System", "Object");

            GetVMContext.Module.Types.Insert(0, newType);

            var old_cctor = oldType.FindOrCreateStaticConstructor();
            var cctor = newType.FindOrCreateStaticConstructor();

            old_cctor.Name = GetVMContext.RTRenamer.GetRandom();
            old_cctor.IsRuntimeSpecialName = false;
            old_cctor.IsSpecialName = false;
            old_cctor.Access = MethodAttributes.Assembly;

            cctor.Body = new CilBody(true, new List<Instruction> {
                Instruction.Create(dnlib.DotNet.Emit.OpCodes.Call, old_cctor),
                Instruction.Create(dnlib.DotNet.Emit.OpCodes.Ret)
            }, new List<ExceptionHandler>(), new List<Local>());

            for (int i = 0; i < oldType.Methods.Count; i++)
            {
                var nativeMethod = oldType.Methods[i];

                if (nativeMethod.IsNative)
                {
                    var methodStub = new MethodDefUser(nativeMethod.Name, nativeMethod.MethodSig.Clone())
                    {
                        Attributes = MethodAttributes.Assembly | MethodAttributes.Static,
                        Body = new CilBody()
                    };

                    methodStub.Body.Instructions.Add(new Instruction(dnlib.DotNet.Emit.OpCodes.Jmp, nativeMethod));
                    methodStub.Body.Instructions.Add(new Instruction(dnlib.DotNet.Emit.OpCodes.Ret));

                    newType.Methods.Add(newType.Methods[i] = methodStub);
                }
            }

            methods.Remove(old_cctor);
            methods.Remove(cctor);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GetVMContext.SetSettings(methods, old_cctor);

            Options = new ModuleWriterOptions(GetVMContext.Module);
            Options.Logger = DummyLogger.NoThrowInstance;
            //Options.MetadataOptions.Flags = MetadataFlags.PreserveEventRids | MetadataFlags.PreserveStringsOffsets;

            Options.WriterEvent += delegate (object sender, ModuleWriterEventArgs e)
            {
                var _writer = (ModuleWriterBase)sender;

                if (_writer.Module == GetVMContext.Module)
                {
                    if (e.Event == ModuleWriterEvent.MDBeginCreateTables)
                    {
                        GetVMContext.InjectMergeRuntime();
                    }
                    else if (e.Event == ModuleWriterEvent.MDBeginWriteMethodBodies)
                    {
                        foreach (var method in GetVMContext.MethodList)
                        {
                            GetVMContext.SetMethod(_writer, method);
                            GetVMContext.Patch();
                        }

                        GetVMContext.Final();
                    }
                }
            };
        }

        public void Final()
        {
            GetVMContext?.Dispose();
        }
    }
}
