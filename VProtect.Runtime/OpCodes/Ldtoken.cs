using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;
using System.Runtime.InteropServices;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldtoken : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldtoken; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.Instructions[opcodeOffset].Operands[0];
            var token = ctx.Instructions[opcodeOffset].Operands[1];

            switch ((byte)type)
            {
                case 0:
                    ctx.Push(new ValueTypeVariant(ctx.GetField((uint)token).FieldHandle));
                    break;
                case 1:
                    ctx.Push(new ValueTypeVariant(ctx.GetMethod((uint)token).MethodHandle));
                    break;
                case 2:
                    ctx.Push(new ValueTypeVariant(ctx.GetType((uint)token).TypeHandle));
                    break;
                default:
                    throw new Exception("Check the instruction. This should not happen.");
            }

            opcodeOffset++;
        }
    }
}
