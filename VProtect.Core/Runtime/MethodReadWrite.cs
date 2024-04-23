using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using VProtect.Core.VM;
using VProtect.Core.OpCodes;
using VProtect.Core.Utilites;
using VProtect.Core.Services;
using VProtect.Core.Services.Injection;

using DOpCodes = dnlib.DotNet.Emit.OpCodes;

namespace VProtect.Core.Runtime
{
    internal class MethodReadWrite : IDisposable
    {
        public VMContext context;

        private MemoryStream vmData;
        private BinaryWriter vmDataWriter;

        public MethodReadWrite(VMContext ctx)
        {
            context = ctx;

            vmData = new MemoryStream();
            vmDataWriter = new BinaryWriter(vmData);
        }

        public void Read()
        {
            var method = context.SelectedMethod;

            if (method == null)
                throw new NullReferenceException("Method cannot be null. Please check context.");

            // Desteklenmeyen opcodeleri öğrenmek için
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                var instr = method.Body.Instructions[i].Synthesize(context);

                if (!OpCodeMap.TryGetOpCode(instr.OpCode, out IOpCode opcode))
                {
                    Console.WriteLine(instr.OpCode);
                }
            }

            using (MemoryStream finalMS = new MemoryStream())
            {
                using (BinaryWriter finalMSWriter = new BinaryWriter(finalMS, Encoding.Unicode))
                {
                    #region Write Method Infos
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    finalMSWriter.Write(method.Name == ".ctor" || method.Name == ".cctor");
                    finalMSWriter.Write(MDTokenResolver.Resolve(context, method));

                    #region Write Locals
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    finalMSWriter.Write(context.SelectedMethodLocals.Count);

                    //foreach (var local in context.SelectedMethodLocals)
                    //    finalMSWriter.Write(MDTokenResolver.Resolve(context, local.Type));
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Write Try Handler
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    finalMSWriter.Write(context.SelectedMethod.Body.ExceptionHandlers.Count);

                    foreach (var bodyExceptionHandler in context.SelectedMethod.Body.ExceptionHandlers.Reverse().ToArray())
                    {
                        var tryStart = bodyExceptionHandler.TryStart;
                        var tryEnd = bodyExceptionHandler.TryEnd;

                        var handlerType = bodyExceptionHandler.HandlerType;
                        var catchType = bodyExceptionHandler.CatchType;

                        var filterStart = bodyExceptionHandler.FilterStart;

                        var handlerStart = bodyExceptionHandler.HandlerStart;
                        var handlerEnd = bodyExceptionHandler.HandlerEnd;

                        // TryStart
                        var tryStartIndex = method.Body.Instructions.IndexOf(tryStart);
                        if (tryStartIndex == -1)
                            finalMSWriter.Write(-1);
                        else
                            finalMSWriter.Write(tryStartIndex);

                        // TryEnd
                        var tryEndIndex = method.Body.Instructions.IndexOf(tryEnd);
                        if (tryEndIndex == -1)
                            finalMSWriter.Write(-1);
                        else
                            finalMSWriter.Write(tryEndIndex);

                        // FilterStart
                        var filterStartIndex = method.Body.Instructions.IndexOf(filterStart);
                        if (filterStartIndex == -1)
                            finalMSWriter.Write(-1);
                        else
                            finalMSWriter.Write(filterStartIndex);

                        // HandlerType
                        finalMSWriter.Write((int)handlerType);

                        // CatchType
                        if (catchType == null)
                            finalMSWriter.Write((uint)0);
                        else
                            finalMSWriter.Write(MDTokenResolver.Resolve(context, catchType));

                        // HandlerStart
                        var handlerStartIndex = method.Body.Instructions.IndexOf(handlerStart);
                        if (handlerStartIndex == -1)
                            finalMSWriter.Write(-1);
                        else
                            finalMSWriter.Write(handlerStartIndex);

                        // HandlerEnd
                        //var handlerEndIndex = method.Body.Instructions.IndexOf(handlerEnd);
                        //if (handlerEndIndex == -1)
                        //    finalMSWriter.Write(-1);
                        //else
                        //    finalMSWriter.Write(handlerEndIndex);
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Write Instructions
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        var instr = method.Body.Instructions[i].Synthesize(context);

                        if (OpCodeMap.TryGetOpCode(instr.OpCode, out IOpCode opcode))
                        {
                            opcode.Run(context, instr.Operand);
                        }
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region VInstructions Write to FinalMS
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    finalMSWriter.Write(context.Instructions.Count);

                    for (int i = 0; i < context.Instructions.Count; i++)
                    {
                        var vinst = context.Instructions[i];

                        finalMSWriter.Write(vinst.ILCode);
                        finalMSWriter.Write(vinst.OperandIndex);

                        for (int o = 0; o < vinst.OperandIndex; o++)
                            finalMSWriter.VWrite(vinst.Operands[o]);
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    vmDataWriter.Write(DataDescriptor.GetExportId(method));
                    vmDataWriter.Write((int)finalMS.Length);
                    vmDataWriter.Write(finalMS.ToArray());
                }
            }
        }

        public void Write()
        {
            var method = context.SelectedMethod;

            if (method == null)
                throw new NullReferenceException("Method cannot be null. Please check context.");

            context.MethodPatcher.Patch(DataDescriptor.GetExportId(method));
        }

        public MemoryStream Final()
        {
            if (vmData.Length != 0)
            {
                var rtOptions = new ModuleWriterOptions(context.RTModule);

                using (MemoryStream stream = new MemoryStream())
                {
                    byte[] mdListIndexsBuffer = BitConverter.GetBytes(DataDescriptor.GetCount());

                    stream.Write(mdListIndexsBuffer, 0, mdListIndexsBuffer.Length);
                    stream.Write(vmData.ToArray(), 0, (int)vmData.Length);

                    rtOptions.WriterEvent += delegate (object sender, ModuleWriterEventArgs e)
                    {
                        var writer = (ModuleWriterBase)sender;

                        if (e.Event == ModuleWriterEvent.MDBeginWriteMethodBodies)
                        {
                            writer.TheOptions.MetadataOptions.MetadataHeapsAdded += delegate (object sender, MetadataHeapsAddedEventArgs e)
                            {
                                e.Heaps.Insert(0, new RawHeap(" ", stream.ToArray()));
                            };
                        }
                    };
                }

                // Remove Mutation Class
                context.RTModule.Types.Remove(context.RTModule.Find("Mutation", true));

                // Save VProtect.Runtime in Stream
                var libBuff = new MemoryStream();
                context.RTModule.Write(libBuff, rtOptions);

                return libBuff;
            }

            return null;
        }

        public void Dispose()
        {
            vmData?.Dispose();
            vmDataWriter?.Close();

            DataDescriptor.ClearList();
        }
    }
}
