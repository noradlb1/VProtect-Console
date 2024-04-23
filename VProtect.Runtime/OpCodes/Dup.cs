using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Dup : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Dup; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.Push(ctx.Peek().Clone());

            opcodeOffset++;
        }
    }
}
