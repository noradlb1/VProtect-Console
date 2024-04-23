using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants.VariantsTypes;

namespace VProtect.Runtime.OpCodes
{
    internal class Convert_Ovf_Un : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Convert_Ovf_Un; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0], ctx.CorlibModule);
            var val = ctx.Pop();
            var convVal = BaseVariant.Convert(val.Conv_ovf(type, true), type);

            ctx.Push(convVal);

            opcodeOffset++;
        }
    }
}
