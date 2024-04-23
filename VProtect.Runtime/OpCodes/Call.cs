using System;
using System.Reflection;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Call : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Call; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var method = ctx.GetMethod((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            var methodType = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[1]);

            var v = ctx.Call(method, methodType, false);
            if (v != null)
                ctx.Push(v);

            opcodeOffset++;
        }
    }
}
