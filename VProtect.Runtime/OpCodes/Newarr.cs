using System;
using System.Reflection;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants;

namespace VProtect.Runtime.OpCodes
{
    internal class Newarr : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Newarr; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            ctx.Push(new ArrayVariant(Array.CreateInstance(type, ctx.Pop().ToInt32())));

            opcodeOffset++;
        }
    }
}
