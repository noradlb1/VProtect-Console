using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldftn : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldftn; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.Push(new MethodVariant(ctx.GetMethod((uint)ctx.Instructions[opcodeOffset].Operands[0])));

            opcodeOffset++;
        }
    }
}
