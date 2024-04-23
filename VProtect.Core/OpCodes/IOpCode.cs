using dnlib.DotNet.Emit;

using VProtect.Core.VM;

namespace VProtect.Core.OpCodes
{
    internal interface IOpCode
    {
        OpCode Code { get; }
        void Run(VMContext ctx, dynamic operand);
    }
}
