using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Endfinally : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Endfinally; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            if (ctx._exception == null)
            {
                opcodeOffset = ctx._finallyStack.Pop();
                return;
            }

            ctx.Unwind(ref opcodeOffset);
        }
    }
}
