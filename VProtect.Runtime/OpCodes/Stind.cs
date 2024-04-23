using System;
using System.Reflection;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants.VariantsTypes;

namespace VProtect.Runtime.OpCodes
{
    internal class Stind : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Stind; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0], ctx.CorlibModule);
            var val2 = ctx.Pop();
            var val1 = ctx.Pop();
            var convVal2 = BaseVariant.Convert(val2, type);

            if (val1.IsReference())
            {
                if (val1.Type() == typeof(VLocal))
                    convVal2 = BaseVariant.Convert(convVal2, val1.Value().GetType());
                else
                    convVal2 = BaseVariant.Convert(convVal2, val1.Type());
            }
            else
            {
                if (val1.Value() is Pointer)
                    unsafe { val1 = new PointerReference(new IntPtr(Pointer.Unbox(val1.Value())), type); }
                else if (val1.Value() is IntPtr)
                    unsafe { val1 = new PointerReference(val1.ToIntPtr(), type); }
                else
                {
                    //Console.WriteLine("dddddddddddddddd");
                    //throw new ArgumentException();
                }
            }

            val1.SetValue(convVal2.Value());

            if (val2.Type() == typeof(VLocal))
                ((LocalVariant)val2).SetLocalValue(ctx, val1.Value());

            ctx.Push(val1);

            opcodeOffset++;
        }
    }
}
