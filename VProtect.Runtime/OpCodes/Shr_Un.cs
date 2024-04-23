using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Shr_Un : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Shr_Un; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var value = ctx.Pop();
            var value1 = ctx.Pop();

            ctx.Push(ctx.Shr(value, value1, true));

            opcodeOffset++;
        }
    }
}
