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
    internal class Shr_Un : IOpCode
    {
        public OpCode Code
        {
            get { return dnlib.DotNet.Emit.OpCodes.Shr_Un; }
        }

        public void Run(VMContext ctx, dynamic operand)
        {
            ctx.Instructions.Add(new VInstruction(VCode.Shr_Un));
        }
    }
}