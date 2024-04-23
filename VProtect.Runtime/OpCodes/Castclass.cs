using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Castclass : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Castclass; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            var v = ctx.Pop();

            if (!Utils.IsInst(v.Value(), type))
                throw new InvalidCastException();

            ctx.Push(v);

            opcodeOffset++;
        }
    }
}
