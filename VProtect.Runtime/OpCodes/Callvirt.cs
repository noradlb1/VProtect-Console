using System;
using System.Linq;
using System.Reflection;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal class Callvirt : IOpCode
    {
        public ushort ILCode
        {
            get { return Constants.Callvirt; }
        }

        public void Run(VMContext ctx, ref int opcodeOffset)
        {
            var method = ctx.GetMethod((uint)ctx.Instructions[opcodeOffset].Operands[0]);
            var methodType = ctx.GetType((uint)ctx.Instructions[opcodeOffset].Operands[1]);

            if (ctx.Constraineds.Count > 0)
            {
                var offset = opcodeOffset;
                var constrained = ctx.Constraineds.FirstOrDefault(c => c.Key == offset);

                if (constrained != null)
                {
                    var parameters = method.GetParameters();
                    var types = new Type[parameters.Length];

                    var num = 0;
                    foreach (var param in parameters)
                        types[num++] = param.ParameterType;

                    var new_method = constrained.Value.GetMethod(method.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty, null, types, null);
                    if (new_method != null)
                        method = new_method;

                    ctx.Constraineds.Remove(constrained);
                }
            }

            var v = ctx.Call(method, methodType, true);
            if (v != null)
                ctx.Push(v);

            opcodeOffset++;
        }
    }
}
