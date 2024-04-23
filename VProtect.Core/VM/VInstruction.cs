using System;

namespace VProtect.Core.VM
{
    internal class VInstruction
    {
        public ushort ILCode
        {
            get;
            private set;
        }

        public object[] Operands
        {
            get;
            private set;
        }

        public int OperandIndex
        {
            get { return Operands.Length; }
        }

        public VInstruction(ushort code, params object[] operands)
        {
            ILCode = code;
            Operands = operands;
        }
    }
}
