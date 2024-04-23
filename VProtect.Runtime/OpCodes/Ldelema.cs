using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldelema : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldelema; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var v = ctx.Pop();
            var array = ctx.Pop().Value() as Array;
            if (array == null)
                throw new ArgumentException();

            ctx.Push(new ArrayElementVariant(array, v.ToInt32()));

            opcodeOffset++;
        }
    }
}
