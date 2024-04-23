using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet;

using VProtect.Core.Services;

namespace VProtect.Core.VM
{
    internal static class DataDescriptor
    {
        static Dictionary<MethodDef, int> exportMap = new Dictionary<MethodDef, int>();
        public static RandomGenerator RandomGenerator;

        static int nextMethodId = 1;

        static DataDescriptor()
        {
            RandomGenerator = new RandomGenerator();
        }

        public static int GetExportId(MethodDef method)
        {
            int ret;
            if (!exportMap.TryGetValue(method, out ret))
            {
                var id = nextMethodId++;
                exportMap[method] = ret = id;
            }

            return ret;
        }

        public static int GetCount()
        {
            return exportMap.Count;
        }

        public static void ClearList()
        {
            nextMethodId = 1;
            exportMap.Clear();
        }
    }
}
