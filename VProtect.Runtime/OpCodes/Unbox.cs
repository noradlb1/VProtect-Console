using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Unbox : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Unbox; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            ctx.Push(BaseVariant.Convert(ctx.Pop().Value(), type));

            opcodeOffset++;
        }
    }
}
