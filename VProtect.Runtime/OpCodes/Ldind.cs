using System;
using System.Reflection;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants.VariantsTypes;

namespace VProtect.Runtime.OpCodes
{
    internal class Ldind : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ldind; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var type = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[0], ctx.CorlibModule);
            
            var v = ctx.Pop();
            var isLocal = v.Type() == typeof(VLocal);

            BaseVariant newV;
            if (!v.IsReference())
            {
                if (v.Value() is Pointer)
                    unsafe { newV = new PointerReference(new IntPtr(Pointer.Unbox(v.Value())), type); }
                else if (v.Value() is IntPtr)
                    unsafe { newV = new PointerReference(v.ToIntPtr(), type); }
                else
                    throw new ArgumentException();
            }
            else
                newV = v;

            var convNewVal = BaseVariant.Convert(newV, type);

            if (isLocal)
            {
                var vLoc = (LocalVariant)v;
                ctx.Push(new LocalVariant(ctx, convNewVal.Value(), type, vLoc.Index(), vLoc.IsReference()));

                vLoc.SetLocalValue(ctx, convNewVal.Value());
            }
            else
                ctx.Push(convNewVal);

            opcodeOffset++;
        }
    }
}
