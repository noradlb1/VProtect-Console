using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Execution.Internal;

namespace VProtect.Runtime.OpCodes
{
    internal class Sizeof : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Sizeof; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.Push(new IntVariant(SizeOfHelper.SizeOf(ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0]))));

            opcodeOffset++;
        }
    }
}
