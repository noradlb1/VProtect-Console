using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ble_Un : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ble_Un; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var value = ctx.Pop();
            var value2 = ctx.Pop();

            if (ctx.Ble(value, value2, true))
                opcodeOffset = (int)ctx.Instructions[opcodeOffset].Operands[0];
            else
            {
                ctx.Push(value);
                ctx.Push(value2);

                opcodeOffset++;
            }
        }
    }
}
