using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Not : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Not; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.Push(ctx.NegAndNot(ctx.Pop(), false));

            opcodeOffset++;
        }
    }
}
