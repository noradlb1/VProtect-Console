using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Brfalse : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Brfalse; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            if (!ctx.Brtrue(ctx.Pop()))
                opcodeOffset = (int)ctx.Instructions[opcodeOffset].Operands[0];
            else
                opcodeOffset++;
        }
    }
}
