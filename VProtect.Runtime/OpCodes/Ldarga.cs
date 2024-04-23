using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldarga : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldarga; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var arg = ctx.Args[(int)ctx.Instructions[opcodeOffset].Operands[0]];
            var argType = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[1]);

            var argVariant = BaseVariant.Convert(arg, argType);
            if (argVariant.IsReference())
                throw new ArgumentException();

            ctx.Push(new ReferenceVariant(argVariant));

            opcodeOffset++;
        }
    }
}
