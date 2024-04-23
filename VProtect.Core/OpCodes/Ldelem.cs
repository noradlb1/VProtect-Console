using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using VProtect.Core.VM;
using VProtect.Core.Utilites;

namespace VProtect.Core.OpCodes
{
    internal class Ldelem : IOpCode
    {
        public OpCode Code
        {
            get { return dnlib.DotNet.Emit.OpCodes.Ldelem; }
        }

        public void Run(VMContext ctx, dynamic operand)
        {
            var convertedOperand = (KeyValuePair<byte, object>)operand;

            if (convertedOperand.Key == 0)
                ctx.Instructions.Add(new VInstruction(VCode.Ldelem, convertedOperand.Key, MDTokenResolver.Resolve(ctx, convertedOperand.Value)));
            else if (convertedOperand.Key == 1)
                ctx.Instructions.Add(new VInstruction(VCode.Ldelem, convertedOperand.Key, MDTokenResolver.GetCodedToken(((TypeDef)convertedOperand.Value).MDToken)));
        }
    }
}