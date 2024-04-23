using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Box : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Box; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            ctx.Push(new ObjectVariant(BaseVariant.Convert(ctx.Pop(), type).Value()));

            opcodeOffset++;
        }
    }
}
