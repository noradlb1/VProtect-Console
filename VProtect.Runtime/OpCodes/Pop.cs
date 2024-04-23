using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Pop : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Pop; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.Pop();

            opcodeOffset++;
        }
    }
}
