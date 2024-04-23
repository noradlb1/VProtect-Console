using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Bge : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Bge; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var value = ctx.Pop();
            var value2 = ctx.Pop();

            if (ctx.Bge(value, value2, false))
                opcodeOffset = (int)ctx.Instructions[opcodeOffset].Operands[0];
            else
            {
                ctx.Push(value);
                ctx.Push(value2);

                opcodeOffset++;
            }
        }
    }
}
