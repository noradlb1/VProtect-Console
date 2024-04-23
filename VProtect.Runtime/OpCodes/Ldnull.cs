using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldnull : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldnull; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            ctx.Push(new ObjectVariant(null));

            opcodeOffset++;
        }
    }
}
