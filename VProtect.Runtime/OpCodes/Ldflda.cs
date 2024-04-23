using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldflda : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldflda; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var field = ctx.GetField((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            var v = ctx.Pop();

            ctx.Push(new FieldReferenceVariant(field, v));

            opcodeOffset++;
        }
    }
}