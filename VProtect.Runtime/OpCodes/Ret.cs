using System;
using System.Reflection;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Ret : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Ret; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            if (ctx.StackLength > 0)
            {
                var value = ctx.Pop();

                if (ctx.VMethodInfo.Method.IsConstructor)
                    ctx.PushValue(null);
                else
                {
                    var methodInfo = (MethodInfo)ctx.VMethodInfo.Method;
                    if (methodInfo.ReturnType != typeof(void))
                    {
                        ctx.PushValue(BaseVariant.Convert(value, methodInfo.ReturnType).Value());
                    }
                    else
                        ctx.PushValue(null);
                }
            }
            else
                ctx.PushValue(null);

            opcodeOffset = ctx.Instructions.Length;
        }
    }
}
