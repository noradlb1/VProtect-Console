using System;
using System.Collections.Generic;

using VProtect.Runtime.OpCodes;

namespace VProtect.Runtime.Data
{
    internal class OpCodeMap
    {
        static readonly Dictionary<ushort, IOpCode> opCodes;

        static OpCodeMap()
        {
            opCodes = new Dictionary<ushort, IOpCode>();

            foreach (var type in typeof(OpCodeMap).Assembly.GetTypes())
            {
                if (typeof(IOpCode).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var opCode = (IOpCode)Activator.CreateInstance(type);

                    opCodes[opCode.ILCode] = opCode;
                }
            }
        }

        public static IOpCode Lookup(ushort code)
        {
            return opCodes[code];
        }
    }
}
