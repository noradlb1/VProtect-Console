using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldc_R8 : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldc_R8; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.Push(new DoubleVariant((double)ctx.Instructions[opcodeOffset].Operands[0]));

            opcodeOffset++;
        }
    }
}
