using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Neg : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Neg; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.Push(ctx.NegAndNot(ctx.Pop(), true));

            opcodeOffset++;
        }
    }
}
