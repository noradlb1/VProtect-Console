using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldlen : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldlen; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var array = ctx.Pop().Value() as Array;
            if (array == null)
                throw new ArgumentException();

            ctx.Push(new IntVariant(array.Length));

            opcodeOffset++;
        }
    }
}
