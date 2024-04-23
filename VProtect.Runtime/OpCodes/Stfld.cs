using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants.VariantsTypes;

namespace VProtect.Runtime.OpCodes
{
    internal class Stfld : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Stfld; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var field = ctx.GetField((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            var value = ctx.Pop();
            var baseVariant = ctx.Pop();

            baseVariant.SetFieldValue(field, BaseVariant.Convert(value, field.FieldType).Value());

            if (baseVariant.Type() == typeof(VLocal))
                ((LocalVariant)baseVariant).SetLocalValue(ctx, baseVariant.Value());

            ctx.Push(baseVariant);

            opcodeOffset++;
        }
    }
}
