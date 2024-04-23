using VProtect.Runtime.Execution;

namespace VProtect.Runtime.OpCodes
{
    internal interface IOpCode
    {
        ushort ILCode { get; }
        void Run(VMContext ctx, ref int opcodeOffset);
    }
}
