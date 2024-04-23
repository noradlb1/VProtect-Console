using System;

namespace VProtect.Runtime.Execution.Internal
{
    internal sealed class CatchBlock
    {
        private byte _type;
        private int _handler;
        private int _filter;
        private Type _catchType;

        public CatchBlock(byte type, int handler, int filter, Type catchType)
        {
            _type = type;
            _handler = handler;
            _filter = filter;
            _catchType = catchType;
        }

        public byte Type()
        {
            return _type;
        }
        public int Handler()
        {
            return _handler;
        }

        public int Filter()
        {
            return _filter;
        }

        public Type CatchType()
        {
            return _catchType;
        }
    }
}
