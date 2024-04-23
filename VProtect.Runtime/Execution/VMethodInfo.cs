using System;
using System.Reflection;

namespace VProtect.Runtime.Execution
{
    internal class VMethodInfo
    {
        public bool IsConstructor;

        public MethodBase Method;
        public int Method_MDToken;

        public Type Type;
        public int TypeToken;

        public object[] Locals;
    }
}
