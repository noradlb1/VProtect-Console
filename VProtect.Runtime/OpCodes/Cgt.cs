using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants;

namespace VProtect.Runtime.OpCodes
{
    internal class Cgt : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Cgt; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var value = ctx.Pop();
            var value2 = ctx.Pop();

            ctx.Push(ctx.CgtAndBgt(value, value2, false));

            opcodeOffset++;
        }
    }
}
