using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Stsfld : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Stsfld; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var field = ctx.GetField((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            var v = ctx.Pop();

            field.SetValue(null, BaseVariant.Convert(v, field.FieldType).Value());

            opcodeOffset++;
        }
    }
}
