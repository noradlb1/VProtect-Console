using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Shl : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Shl; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var value = ctx.Pop();
            var value1 = ctx.Pop();

            ctx.Push(ctx.Shl(value, value1));

            opcodeOffset++;
        }
    }
}
