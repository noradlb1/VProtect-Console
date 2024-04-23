using System;
using System.Collections;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Leave : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Leave; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx._finallyStack.Push((int)ctx.Instructions[opcodeOffset].Operands[0]);

            while (ctx._tryStack.Count != 0 && opcodeOffset >= ctx._tryStack.Peek().Begin())
            {
                var tryBlock = ctx._tryStack.PopFromFirst();
                var catchBlocks = tryBlock.CatchBlocks();

                for (int i = 0; i < catchBlocks.Count; i++)
                {
                    var current = catchBlocks[i];
                    if (current.Type() == 2)
                        ctx._finallyStack.Push(current.Handler());
                }
            }

            ctx._exception = null;
            ctx.Clear();

            opcodeOffset = ctx._finallyStack.Pop();
        }
    }
}
