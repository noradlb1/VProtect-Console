using System;
using System.Collections.Generic;

using dnlib.DotNet.Emit;

using VProtect.Core.OpCodes;

namespace VProtect.Core.VM
{
    internal class OpCodeMap
    {
        static readonly Dictionary<OpCode, IOpCode> opCodes;

        static OpCodeMap()
        {
            opCodes = new Dictionary<OpCode, IOpCode>();

            foreach (var type in typeof(OpCodeMap).Assembly.GetTypes())
            {
                if (typeof(IOpCode).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var opCode = (IOpCode)Activator.CreateInstance(type);
                  
                    opCodes[opCode.Code] = opCode;
                }
            }
        }

        public static bool TryGetOpCode(OpCode code, out IOpCode opcode)
        {
            if (opCodes.TryGetValue(code, out IOpCode _val))
            {
                opcode = _val;
                return true;
            }
            else
            {
                opcode = null;
                return false;
            }
        }
    }
}
