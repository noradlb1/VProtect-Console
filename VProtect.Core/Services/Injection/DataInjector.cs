using System;
using System.Linq;
using System.Text;

using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using VProtect.Core.VM;
using VProtect.Core.Runtime;

using DOpCodes = dnlib.DotNet.Emit.OpCodes;

namespace VProtect.Core.Services.Injection
{
    internal static class DataInjector
    {
        public static void InjectRawBytes(MethodDef target, FieldDef field, byte[] bytes, int i = 0)
        {
            var module = target.Module;
            var instructions = target.Body.Instructions;
            var count = 2;

            instructions.Insert(i, DOpCodes.Ldc_I4.ToInstruction(bytes.Length));
            instructions.Insert(i + 1, DOpCodes.Newarr.ToInstruction(module.CorLibTypes.Byte));
            instructions.Insert(i + 2, DOpCodes.Dup.ToInstruction());

            for (int j = 0; j < bytes.Length; j++)
            {
                var value = Convert.ToInt32(bytes[j]);
                instructions.Insert(i + ++count, DOpCodes.Ldc_I4.ToInstruction(j));
                instructions.Insert(i + ++count, DOpCodes.Ldc_I4.ToInstruction(value));
                instructions.Insert(i + ++count, DOpCodes.Stelem_I1.ToInstruction());
                instructions.Insert(i + ++count, DOpCodes.Dup.ToInstruction());
            }

            instructions.Insert(i + ++count, DOpCodes.Pop.ToInstruction());
            instructions.Insert(i + count, DOpCodes.Stsfld.ToInstruction(field));

        }
         
        public static KeyValuePair<TypeDef, FieldDef> InjectBytes(MethodDef target, RTRenamer renamer, byte[] bytes)
        {
            var type = new TypeDefUser(renamer.GetRandom(), target.Module.CorLibTypes.GetTypeRef("System", "ValueType"))
            {
                Layout = TypeAttributes.ExplicitLayout,
                Visibility = TypeAttributes.Sealed,
                IsSealed = true,
                ClassLayout = new ClassLayoutUser(0, (uint)bytes.Length)
            };

            target.Module.Types.Add(type);

            var field = new FieldDefUser(renamer.GetRandom(), new FieldSig(type.ToTypeSig()), FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.HasFieldRVA)
            {
                HasFieldRVA = true,
                InitialValue = bytes
            };

            target.DeclaringType.Fields.Add(field);

            return new KeyValuePair<TypeDef, FieldDef>(type, field);
        }
    }
}
