using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using VProtect.Core.VM;

using DOpCodes = dnlib.DotNet.Emit.OpCodes;

namespace VProtect.Core.Runtime
{
    internal class MethodPatcher
    {
        private VMContext context;

        public MethodPatcher(VMContext context)
        {
            this.context = context;
        }

        public void Patch(int id)
        {
            var body = new CilBody();
            context.SelectedMethod.Body = body;

            body.Instructions.Add(Instruction.Create(DOpCodes.Ldtoken, context.SelectedMethod.DeclaringType));
            body.Instructions.Add(Instruction.Create(DOpCodes.Ldc_I4, id));

            body.Instructions.Add(Instruction.Create(DOpCodes.Ldc_I4, context.SelectedMethod.Parameters.Count));
            body.Instructions.Add(Instruction.Create(DOpCodes.Newarr, context.SelectedMethod.Module.CorLibTypes.Object.ToTypeDefOrRef()));

            foreach (var param in context.SelectedMethod.Parameters)
            {
                body.Instructions.Add(Instruction.Create(DOpCodes.Dup));
                body.Instructions.Add(Instruction.Create(DOpCodes.Ldc_I4, param.Index));
                body.Instructions.Add(Instruction.Create(DOpCodes.Ldarg, param));

                if (param.Type.IsValueType)
                    body.Instructions.Add(Instruction.Create(DOpCodes.Box, param.Type.ToTypeDefOrRef()));
                else if (param.Type.IsPointer)
                {
                    body.Instructions.Add(Instruction.Create(DOpCodes.Conv_U));
                    body.Instructions.Add(Instruction.Create(DOpCodes.Box, context.Module.CorLibTypes.UIntPtr.ToTypeDefOrRef()));
                }

                body.Instructions.Add(Instruction.Create(DOpCodes.Stelem_Ref));
            }

            body.Instructions.Add(Instruction.Create(DOpCodes.Call, context.Module.Import(context.RTSearch.Entry_Execute)));

            if (context.SelectedMethod.ReturnType.ElementType == ElementType.Void)
                body.Instructions.Add(Instruction.Create(DOpCodes.Pop));
            else if (context.SelectedMethod.ReturnType.IsValueType)
                body.Instructions.Add(Instruction.Create(DOpCodes.Unbox_Any, context.SelectedMethod.ReturnType.ToTypeDefOrRef()));
            else
                body.Instructions.Add(Instruction.Create(DOpCodes.Castclass, context.SelectedMethod.ReturnType.ToTypeDefOrRef()));

            body.Instructions.Add(Instruction.Create(DOpCodes.Ret));

            body.OptimizeMacros();

            //context.GetMethod.Body.KeepOldMaxStack = true;
        }
    }
}