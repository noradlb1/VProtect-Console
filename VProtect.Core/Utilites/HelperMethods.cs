using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using VProtect.Core.VM;
using VProtect.Core.Services;

using DOpCode = dnlib.DotNet.Emit.OpCode;
using DOpCodes = dnlib.DotNet.Emit.OpCodes;

namespace VProtect.Core.Utilites
{
    internal static class HelperMethods
    {
        public static readonly char[] hexCharset = "0123456789ABCDEF".ToCharArray();

        public static Instruction Synthesize(this Instruction instruction, VMContext ctx)
        {
            var instr = instruction.Clone();

            switch (instr.OpCode.Code)
            {
                case Code.Unbox_Any:
                    instr.OpCode = DOpCodes.Unbox;
                    break;


                case Code.Ldarg_0:
                    instr.OpCode = DOpCodes.Ldarg;
                    instr.Operand = ctx.SelectedMethod.Parameters[0];
                    break;
                case Code.Ldarg_1:
                    instr.OpCode = DOpCodes.Ldarg;
                    instr.Operand = ctx.SelectedMethod.Parameters[1];
                    break;
                case Code.Ldarg_2:
                    instr.OpCode = DOpCodes.Ldarg;
                    instr.Operand = ctx.SelectedMethod.Parameters[2];
                    break;
                case Code.Ldarg_3:
                    instr.OpCode = DOpCodes.Ldarg;
                    instr.Operand = ctx.SelectedMethod.Parameters[3];
                    break;
                case Code.Ldarg_S:
                    instr.OpCode = DOpCodes.Ldarg;
                    break;
                case Code.Ldarga_S:
                    instr.OpCode = DOpCodes.Ldarga;
                    break;


                case Code.Ldsfld:
                    instr.OpCode = DOpCodes.Ldfld;
                    break;
                case Code.Ldsflda:
                    instr.OpCode = DOpCodes.Ldflda;
                    break;


                case Code.Ldc_I4_0:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = 0;
                    break;
                case Code.Ldc_I4_1:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = 1;
                    break;
                case Code.Ldc_I4_2:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = 2;
                    break;
                case Code.Ldc_I4_3:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = 3;
                    break;
                case Code.Ldc_I4_4:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = 4;
                    break;
                case Code.Ldc_I4_5:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = 5;
                    break;
                case Code.Ldc_I4_6:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = 6;
                    break;
                case Code.Ldc_I4_7:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = 7;
                    break;
                case Code.Ldc_I4_8:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = 8;
                    break;
                case Code.Ldc_I4_M1:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    instr.Operand = -1;
                    break;
                case Code.Ldc_I4_S:
                    instr.OpCode = DOpCodes.Ldc_I4;
                    break;


                case Code.Br_S:
                    instr.OpCode = DOpCodes.Br;
                    break;
                case Code.Brtrue_S:
                    instr.OpCode = DOpCodes.Brtrue;
                    break;
                case Code.Brfalse_S:
                    instr.OpCode = DOpCodes.Brfalse;
                    break;


                case Code.Beq_S:
                    instr.OpCode = DOpCodes.Beq;
                    break;


                case Code.Bne_Un_S:
                    instr.OpCode = DOpCodes.Bne_Un;
                    break;


                case Code.Bge_S:
                    instr.OpCode = DOpCodes.Bge;
                    break;
                case Code.Bge_Un_S:
                    instr.OpCode = DOpCodes.Bge_Un;
                    break;


                case Code.Ble_S:
                    instr.OpCode = DOpCodes.Ble;
                    break;
                case Code.Ble_Un_S:
                    instr.OpCode = DOpCodes.Ble_Un;
                    break;


                case Code.Bgt_S:
                    instr.OpCode = DOpCodes.Bgt;
                    break;
                case Code.Bgt_Un_S:
                    instr.OpCode = DOpCodes.Bgt_Un;
                    break;


                case Code.Blt_S:
                    instr.OpCode = DOpCodes.Blt;
                    break;
                case Code.Blt_Un_S:
                    instr.OpCode = DOpCodes.Blt_Un;
                    break;


                case Code.Stloc_0:
                    instr.OpCode = DOpCodes.Stloc;
                    instr.Operand = ctx.SelectedMethodLocals[0];
                    break;
                case Code.Stloc_1:
                    instr.OpCode = DOpCodes.Stloc;
                    instr.Operand = ctx.SelectedMethodLocals[1];
                    break;
                case Code.Stloc_2:
                    instr.OpCode = DOpCodes.Stloc;
                    instr.Operand = ctx.SelectedMethodLocals[2];
                    break;
                case Code.Stloc_3:
                    instr.OpCode = DOpCodes.Stloc;
                    instr.Operand = ctx.SelectedMethodLocals[3];
                    break;
                case Code.Stloc_S:
                    instr.OpCode = DOpCodes.Stloc;
                    break;


                case Code.Ldloc_0:
                    instr.OpCode = DOpCodes.Ldloc;
                    instr.Operand = ctx.SelectedMethodLocals[0];
                    break;
                case Code.Ldloc_1:
                    instr.OpCode = DOpCodes.Ldloc;
                    instr.Operand = ctx.SelectedMethodLocals[1];
                    break;
                case Code.Ldloc_2:
                    instr.OpCode = DOpCodes.Ldloc;
                    instr.Operand = ctx.SelectedMethodLocals[2];
                    break;
                case Code.Ldloc_3:
                    instr.OpCode = DOpCodes.Ldloc;
                    instr.Operand = ctx.SelectedMethodLocals[3];
                    break;
                case Code.Ldloc_S:
                    instr.OpCode = DOpCodes.Ldloc;
                    break;
                case Code.Ldloca_S:
                    instr.OpCode = DOpCodes.Ldloca;
                    break;


                case Code.Stind_I:
                    instr.OpCode = DOpCodes.Stind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "IntPtr").ResolveTypeDef();
                    break;
                case Code.Stind_I1:
                    instr.OpCode = DOpCodes.Stind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "SByte").ResolveTypeDef();
                    break;
                case Code.Stind_I2:
                    instr.OpCode = DOpCodes.Stind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int16").ResolveTypeDef();
                    break;
                case Code.Stind_I4:
                    instr.OpCode = DOpCodes.Stind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int32").ResolveTypeDef();
                    break;
                case Code.Stind_I8:
                    instr.OpCode = DOpCodes.Stind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int64").ResolveTypeDef();
                    break;
                case Code.Stind_R4:
                    instr.OpCode = DOpCodes.Stind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Single").ResolveTypeDef();
                    break;
                case Code.Stind_R8:
                    instr.OpCode = DOpCodes.Stind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Double").ResolveTypeDef();
                    break;
                case Code.Stind_Ref:
                    instr.OpCode = DOpCodes.Stind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Object").ResolveTypeDef();
                    break;


                case Code.Ldind_I:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "IntPtr").ResolveTypeDef();
                    break;
                case Code.Ldind_I1:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "SByte").ResolveTypeDef();
                    break;
                case Code.Ldind_I2:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int16").ResolveTypeDef();
                    break;
                case Code.Ldind_I4:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int32").ResolveTypeDef();
                    break;
                case Code.Ldind_I8:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int64").ResolveTypeDef();
                    break;
                case Code.Ldind_R4:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Single").ResolveTypeDef();
                    break;
                case Code.Ldind_R8:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Double").ResolveTypeDef();
                    break;
                case Code.Ldind_U1:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Byte").ResolveTypeDef();
                    break;
                case Code.Ldind_U2:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt16").ResolveTypeDef();
                    break;
                case Code.Ldind_U4:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt32").ResolveTypeDef();
                    break;
                case Code.Ldind_Ref:
                    instr.OpCode = DOpCodes.Ldind_I;
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Object").ResolveTypeDef();
                    break;


                case Code.Stelem:
                    instr.OpCode = DOpCodes.Stelem;
                    instr.Operand = new KeyValuePair<byte, object>(0, instr.Operand);
                    break;
                case Code.Stelem_I:
                    instr.OpCode = DOpCodes.Stelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "IntPtr").ResolveTypeDef());
                    break;
                case Code.Stelem_I1:
                    instr.OpCode = DOpCodes.Stelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "SByte").ResolveTypeDef());
                    break;
                case Code.Stelem_I2:
                    instr.OpCode = DOpCodes.Stelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Int16").ResolveTypeDef());
                    break;
                case Code.Stelem_I4:
                    instr.OpCode = DOpCodes.Stelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Int32").ResolveTypeDef());
                    break;
                case Code.Stelem_I8:
                    instr.OpCode = DOpCodes.Stelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Int64").ResolveTypeDef());
                    break;
                case Code.Stelem_R4:
                    instr.OpCode = DOpCodes.Stelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Single").ResolveTypeDef());
                    break;
                case Code.Stelem_R8:
                    instr.OpCode = DOpCodes.Stelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Double").ResolveTypeDef());
                    break;
                case Code.Stelem_Ref:
                    instr.OpCode = DOpCodes.Stelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Object").ResolveTypeDef());
                    break;


                case Code.Ldelem:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(0, instr.Operand);
                    break;
                case Code.Ldelem_I:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "IntPtr").ResolveTypeDef());
                    break;
                case Code.Ldelem_I1:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "SByte").ResolveTypeDef());
                    break;
                case Code.Ldelem_I2:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Int16").ResolveTypeDef());
                    break;
                case Code.Ldelem_I4:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Int32").ResolveTypeDef());
                    break;
                case Code.Ldelem_I8:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Int64").ResolveTypeDef());
                    break;
                case Code.Ldelem_R4:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Single").ResolveTypeDef());
                    break;
                case Code.Ldelem_R8:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Double").ResolveTypeDef());
                    break;
                case Code.Ldelem_U1:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Byte").ResolveTypeDef());
                    break;
                case Code.Ldelem_U2:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "UInt16").ResolveTypeDef());
                    break;
                case Code.Ldelem_U4:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "UInt32").ResolveTypeDef());
                    break;
                case Code.Ldelem_Ref:
                    instr.OpCode = DOpCodes.Ldelem;
                    instr.Operand = new KeyValuePair<byte, object>(1, ctx.Module.CorLibTypes.GetTypeRef("System", "Object").ResolveTypeDef());
                    break;


                case Code.Conv_I:
                    instr.OpCode = DOpCodes.Conv_I; // IntPtr
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "IntPtr").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_I1:
                    instr.OpCode = DOpCodes.Conv_I; // sbyte
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "SByte").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_I2:
                    instr.OpCode = DOpCodes.Conv_I; // short
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int16").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_I4:
                    instr.OpCode = DOpCodes.Conv_I; // int
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int32").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_I8:
                    instr.OpCode = DOpCodes.Conv_I; // long
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int64").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_R4:
                    instr.OpCode = DOpCodes.Conv_I; // float
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Single").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_R8:
                    instr.OpCode = DOpCodes.Conv_I; // double
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Double").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_U:
                    instr.OpCode = DOpCodes.Conv_I; // UIntPtr
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UIntPtr").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_U1:
                    instr.OpCode = DOpCodes.Conv_I; // byte
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Byte").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_U2:
                    instr.OpCode = DOpCodes.Conv_I; // ushort
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt16").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_U4:
                    instr.OpCode = DOpCodes.Conv_I; // uint
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt32").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_U8:
                    instr.OpCode = DOpCodes.Conv_I; // ulong
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt64").ResolveTypeDef().MDToken;
                    break;

                // buna baktır
                //case Code.Conv_R_Un:
                //    instr.OpCode = DOpCodes.Conv_I;
                //    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Single").ResolveTypeDef().MDToken;
                //    break;


                case Code.Conv_Ovf_I:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // IntPtr
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "IntPtr").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_I1:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // sbyte
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "SByte").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_I2:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // short
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int16").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_I4:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // int
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int32").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_I8:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // long
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int64").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // UIntPtr
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UIntPtr").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U1:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // byte
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Byte").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U2:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // ushort
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt16").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U4:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // uint
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt32").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U8:
                    instr.OpCode = DOpCodes.Conv_Ovf_I; // ulong
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt64").ResolveTypeDef().MDToken;
                    break;


                case Code.Conv_Ovf_I_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // IntPtr
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "IntPtr").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_I1_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // sbyte
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "SByte").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_I2_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // short
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Single").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_I4_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // int
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int32").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_I8_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // long
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Int64").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // UIntPtr
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UIntPtr").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U1_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // byte
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "Byte").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U2_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // ushort
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt16").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U4_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // uint
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt32").ResolveTypeDef().MDToken;
                    break;
                case Code.Conv_Ovf_U8_Un:
                    instr.OpCode = DOpCodes.Conv_Ovf_I_Un; // ulong
                    instr.Operand = ctx.Module.CorLibTypes.GetTypeRef("System", "UInt64").ResolveTypeDef().MDToken;
                    break;


                case Code.Leave_S:
                    instr.OpCode = DOpCodes.Leave;
                    break;
                default:
                    break;
            }

            return instr;
        }

        public static void VWrite(this BinaryWriter writer, object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean:
                    writer.Write((sbyte)0);
                    writer.Write((bool)value);
                    break;
                case TypeCode.Char:
                    writer.Write((sbyte)1);
                    writer.Write((char)value);
                    break;
                case TypeCode.Byte:
                    writer.Write((sbyte)2);
                    writer.Write((byte)value);
                    break;
                case TypeCode.SByte:
                    writer.Write((sbyte)3);
                    writer.Write((sbyte)value);
                    break;
                case TypeCode.Int16:
                    writer.Write((sbyte)4);
                    writer.Write((short)value);
                    break;
                case TypeCode.UInt16:
                    writer.Write((sbyte)5);
                    writer.Write((ushort)value);
                    break;
                case TypeCode.Int32:
                    writer.Write((sbyte)6);
                    writer.Write((int)value);
                    break;
                case TypeCode.UInt32:
                    writer.Write((sbyte)7);
                    writer.Write((uint)value);
                    break;
                case TypeCode.Int64:
                    writer.Write((sbyte)8);
                    writer.Write((long)value);
                    break;
                case TypeCode.UInt64:
                    writer.Write((sbyte)9);
                    writer.Write((ulong)value);
                    break;
                case TypeCode.Single:
                    writer.Write((sbyte)10);
                    writer.Write((float)value);
                    break;
                case TypeCode.Double:
                    writer.Write((sbyte)11);
                    writer.Write((double)value);
                    break;
                case TypeCode.Decimal:
                    writer.Write((sbyte)12);
                    writer.Write((decimal)value);
                    break;
                case TypeCode.String:
                    writer.Write((sbyte)13);
                    writer.Write((string)value);
                    break;
                default:
                    break;
            }
        }

        public static void Shuffle<T>(this RandomGenerator random, IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.NextInt32(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void AddListEntry<TKey, TValue>(this IDictionary<TKey, List<TValue>> self, TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            List<TValue> list;
            if (!self.TryGetValue(key, out list))
                list = self[key] = new List<TValue>();

            list.Add(value);
        }

        public static IList<T> RemoveWhere<T>(this IList<T> self, Predicate<T> match)
        {
            for (int i = self.Count - 1; i >= 0; i--)
            {
                if (match(self[i]))
                    self.RemoveAt(i);
            }
            return self;
        }

        public static void AddRange<T>(this IList<T> list, IList<T> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                list.Add(values[i]);
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defValue = default(TValue))
        {
            TValue ret;
            if (dictionary.TryGetValue(key, out ret))
                return ret;
            return defValue;
        }

        public static TValue GetValueOrDefaultLazy<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TKey, TValue> defValueFactory)
        {
            TValue ret;
            if (dictionary.TryGetValue(key, out ret))
                return ret;
            return defValueFactory(key);
        }

        public static string ToHexString(this byte[] buff)
        {
            var ret = new char[buff.Length * 2];
            int i = 0;
            foreach (byte val in buff)
            {
                ret[i++] = hexCharset[val >> 4];
                ret[i++] = hexCharset[val & 0xf];
            }
            return new string(ret);
        }

        public static string ToHexString(this string str)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
    }
}
