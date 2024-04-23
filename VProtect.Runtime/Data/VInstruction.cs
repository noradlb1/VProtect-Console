namespace VProtect.Runtime.Data
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

        public VInstruction(ushort code, object[] operands)
        {
            ILCode = code;
            Operands = operands;
        }
    }
}
