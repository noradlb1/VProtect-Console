using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Stelem : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Stelem; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = (byte)ctx.Instructions[opcodeOffset].Operands[0];
            var mdtoken = (uint)ctx.Instructions[opcodeOffset].Operands[1];

            Type ldlemType = null;
            if (type == 0)
                ldlemType = ctx.GetType(mdtoken);
            else if (type == 1)
                ldlemType = ctx.GetType(mdtoken, ctx.CorlibModule);

            var v2 = ctx.Pop();
            var v1 = ctx.Pop();
            var array = ctx.Pop().Value() as Array;
            if (array == null)
                throw new ArgumentException();

            array.SetValue(BaseVariant.Convert(BaseVariant.Convert(v2, ldlemType), array.GetType().GetElementType()).Value(), v1.ToInt32());

            opcodeOffset++;
        }
    }
}
