using System;
using System.Collections;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants;

namespace VProtect.Runtime.OpCodes
{
    internal class Endfilter : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Endfilter; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            if (ctx.Pop().ToInt32() != 0)
            {
                ctx._tryStack.PopFromEnd();
                ctx.Push(new ObjectVariant(ctx._exception));

                opcodeOffset = ctx._filterBlock.Handler();
               
                ctx._filterBlock = null;
                return;
            }

            ctx.Unwind(ref opcodeOffset);
        }
    }
}
