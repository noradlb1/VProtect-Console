using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldloca : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldloca; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var localIndex = (int)ctx.Instructions[opcodeOffset].Operands[0];
            var localValue = ctx.VMethodInfo.Locals[localIndex];
            var localType = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[1]);

            ctx.Push(new LocalVariant(ctx, localValue, localType, localIndex, true));

            opcodeOffset++;
        }
    }
}
