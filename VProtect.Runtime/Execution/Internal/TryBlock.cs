using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VProtect.Runtime.Execution.Internal
{
    internal sealed class TryBlock
    {
        private int _begin;
        private int _end;
        private List<CatchBlock> _catchBlocks = new List<CatchBlock>();

        public TryBlock(int begin, int end)
        {
            _begin = begin;
            _end = end;
        }

        public int Begin()
        {
            return _begin;
        }
        public int End()
        {
            return _end;
        }

        public int CompareTo(TryBlock compare)
        {
            if (compare == null)
                return 1;

            int res = _begin.CompareTo(compare.Begin());
            if (res == 0)
                res = compare.End().CompareTo(_end);
            return res;
        }

        public void AddCatchBlock(byte type, int handler, int filter, Type catchType)
        {
            _catchBlocks.Add(new CatchBlock(type, handler, filter, catchType));
        }

        public List<CatchBlock> CatchBlocks()
        {
            return _catchBlocks;
        }
    }
}
