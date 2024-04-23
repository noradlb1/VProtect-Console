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
    internal class Ldtoken : IOpCode
    {
        public OpCode Code
        {
            get { return dnlib.DotNet.Emit.OpCodes.Ldtoken; }
        }

        public void Run(VMContext ctx, dynamic operand)
        {
            if (operand == null)
                throw new Exception("Check the instruction. This should not happen");

            byte type;
            if (operand is IField)
                type = 0;
            else if (operand is IMethod || operand is IMethodDefOrRef)
                type = 1;
            else if (operand is IType || operand is ITypeDefOrRef)
                type = 2;
            else
                throw new Exception("Check the instruction. This should not happen");

            ctx.Instructions.Add(new VInstruction(VCode.Ldtoken, type, MDTokenResolver.Resolve(ctx, operand)));
        }
    }
}