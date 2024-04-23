using System;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;
using System.Runtime.Serialization;

namespace VProtect.Runtime.OpCodes
{
    internal class Initobj : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Initobj; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            var v = ctx.Pop();

            object obj = null;
            if (type.IsValueType)
                if (Nullable.GetUnderlyingType(type) == null)
                    obj = FormatterServices.GetUninitializedObject(type);

            v.SetValue(obj);

            ctx.Push(v);

            opcodeOffset++;
        }
    }
}
