using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldfld : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldfld; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var field = ctx.GetField((uint)ctx.Instructions[opcodeOffset].Operands[0]);

            object obj;
            if (!field.IsStatic)
            {
                var v = ctx.Pop();

                if (v.Type().IsPointer)
                    obj = Marshal.PtrToStructure(v.ToIntPtr(), v.Type().GetElementType());
                else
                    obj = v.Value();
            }
            else
                obj = null;

            ctx.Push(BaseVariant.Convert(field.GetValue(obj), field.FieldType));

            opcodeOffset++;
        }
    }
}