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
    internal class Ldarga : IOpCode
    {
        public OpCode Code
        {
            get { return dnlib.DotNet.Emit.OpCodes.Ldarga; }
        }

        public void Run(VMContext ctx, dynamic operand)
        {
            var paramType = ((Parameter)operand).Type;
            ctx.Instructions.Add(new VInstruction(VCode.Ldarga, ctx.SelectedMethod.Parameters.IndexOf((Parameter)operand), MDTokenResolver.Resolve(ctx, paramType)));
        }
    }
}