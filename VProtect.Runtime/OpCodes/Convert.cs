using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Execution.Internal;
using VProtect.Runtime.Variants.VariantsTypes;

namespace VProtect.Runtime.OpCodes
{
    internal class Convert : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Convert; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var val = ctx.Pop();
            var convVal = ConvHelper.Convert(val.Value(), System.Reflection.Emit.OpCodes.Conv_U);
            var convVar = BaseVariant.Convert(convVal, convVal.GetType());

            if (val.Type() == typeof(VLocal))
                ((LocalVariant)val).SetLocalValue(ctx, convVar.Value());

            ctx.Push(convVar);

            opcodeOffset++;
        }
    }
}
