using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ceq : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ceq; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var val = ctx.Pop();
            var val2 = ctx.Pop();

            ctx.Push(ctx.CeqAndBeq(val, val2));

            opcodeOffset++;
        }
    }
}
