using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Stloc : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Stloc; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.VMethodInfo.Locals[(int)ctx.Instructions[opcodeOffset].Operands[0]] = ctx.Pop().Value();

            opcodeOffset++;
        }
    }
}
