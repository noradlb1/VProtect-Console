using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldstr : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldstr; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.Push(new StringVariant((string)ctx.Instructions[opcodeOffset].Operands[0]));

            opcodeOffset++;
        }
    }
}
