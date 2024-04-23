using System;
using System.Collections;
using System.Reflection.Emit;

namespace VProtect.Runtime.Execution.Internal
{
    internal class ConvHelper
    {
        public static object Convert<T>(T val, OpCode code)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(T), new[] { typeof(T) }, Unverifier.Module, true);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

            ilGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
            ilGenerator.Emit(code);
            ilGenerator.Emit(System.Reflection.Emit.OpCodes.Ret);

            return dynamicMethod.Invoke(null, new object[] { val });
        }
    }
}