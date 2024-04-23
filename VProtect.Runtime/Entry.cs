using System;
using System.Runtime.InteropServices;

namespace VProtect.Runtime
{
    public static class Entry
    {
        public static object Execute(RuntimeTypeHandle typeHandle, int id, object[] args)
        {
            var type = Type.GetTypeFromHandle(typeHandle);
            return VMInstance.Instance(type.Module).Execute(id, args);
        }
    }
}
