using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VProtect.Runtime.Execution.Internal
{
    internal sealed class UnmanagedObjectHandler : IDisposable
    {
        private List<GCHandle> _gchandles = new List<GCHandle>();

        public IntPtr PinObject(object obj)
        {
            GCHandle gchandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            _gchandles.Add(gchandle);

            return gchandle.AddrOfPinnedObject();
        }

        public void Dispose()
        {
            foreach (GCHandle gchandle in _gchandles)
                gchandle.Free();

            _gchandles.Clear();
        }
    }
}
