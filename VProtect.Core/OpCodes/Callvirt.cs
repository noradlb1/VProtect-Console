using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using VProtect.Core.VM;
using VProtect.Core.Utilites;

namespace VProtect.Core.OpCodes
{
    internal class Callvirt : IOpCode
    {
        public OpCode Code
        {
            get { return dnlib.DotNet.Emit.OpCodes.Callvirt; }
        }

        public void Run(VMContext ctx, dynamic operand)
        {
            var method = operand is IMethod ? operand as IMethod : operand as IMethodDefOrRef;
            ctx.Instructions.Add(new VInstruction(VCode.Callvirt, MDTokenResolver.Resolve(ctx, method), MDTokenResolver.Resolve(ctx, method.DeclaringType)));
        }
    }
}