using System;
using System.Reflection;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Newobj : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Newobj; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var constr = ctx.GetMethod((uint)ctx.Instructions[opcodeOffset].Operands[0]);

            var v = ctx.Newobj(constr);
            if (v != null)
                ctx.Push(v);

            opcodeOffset++;
        }
    }
}
