using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Div_Un : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Div_Un; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var value = ctx.Pop();
            var value1 = ctx.Pop();

            ctx.Push(ctx.Div(value, value1, true));

            opcodeOffset++;
        }
    }
}
