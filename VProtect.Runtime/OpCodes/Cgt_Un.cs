using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants;

namespace VProtect.Runtime.OpCodes
{
    internal class Cgt_Un : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Cgt_Un; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var value = ctx.Pop();
            var value2 = ctx.Pop();

            ctx.Push(ctx.CgtAndBgt(value, value2, true));

            opcodeOffset++;
        }
    }
}
