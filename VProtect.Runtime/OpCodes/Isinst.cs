using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Isinst : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Isinst; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            var v = ctx.Pop();

            ctx.Push(new BoolVariant(Utils.IsInst(v.Value(), type)));

            opcodeOffset++;
        }
    }
}
