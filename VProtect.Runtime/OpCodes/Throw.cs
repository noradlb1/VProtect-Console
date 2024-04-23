using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Throw : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Throw; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var exception = ctx.Pop().Value() as Exception;
            if (exception == null)
                throw new ArgumentException();

            opcodeOffset++;
            throw exception;
        }
    }
}
