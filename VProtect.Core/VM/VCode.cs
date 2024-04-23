#pragma warning disable 0649

namespace VProtect.Core.VM
{
    internal static class VCode
    {
        public static ushort Nop = 0;
        public static ushort Pop = 1;
        public static ushort Dup = 2;
        public static ushort Ldnull = 3;

        public static ushort Ldarg = 4;
        public static ushort Ldarga = 5;

        public static ushort Ldtoken = 6;
        public static ushort Call = 7;
        public static ushort Callvirt = 8;
        public static ushort Newobj = 9;
        public static ushort Newarr = 10;
        public static ushort Ldftn = 11;
        public static ushort Initobj = 12;
        public static ushort Constrained = 13;
        public static ushort Box = 14;
        public static ushort Unbox = 15;
        public static ushort Castclass = 16;
        public static ushort Isinst = 17;

        public static ushort Ldelem = 18;
        public static ushort Ldelema = 19;
        public static ushort Stelem = 20;

        public static ushort Ldfld = 21;
        public static ushort Ldflda = 22;

        public static ushort Stfld = 23;
        public static ushort Stsfld = 24;

        public static ushort Stloc = 25;

        public static ushort Ldloc = 26;
        public static ushort Ldloca = 27;

        public static ushort Ldstr = 28;
        public static ushort Ldc_I4 = 29;
        public static ushort Ldc_I8 = 30;
        public static ushort Ldc_R4 = 31;
        public static ushort Ldc_R8 = 32;

        public static ushort Sizeof = 33;

        public static ushort Or = 34;
        public static ushort Not = 35;
        public static ushort Xor = 36;
        public static ushort Shl = 37;
        public static ushort And = 38;
        public static ushort Div = 39;
        public static ushort Div_Un = 40;
        public static ushort Rem = 41;
        public static ushort Rem_Un = 42;
        public static ushort Add = 43;
        public static ushort Add_Ovf = 44;
        public static ushort Add_Ovf_Un = 45;
        public static ushort Sub = 46;
        public static ushort Sub_Ovf = 47;
        public static ushort Sub_Ovf_Un = 48;
        public static ushort Mul = 49;
        public static ushort Mul_Ovf = 50;
        public static ushort Mul_Ovf_Un = 51;
        public static ushort Shr = 52;
        public static ushort Shr_Un = 53;
        public static ushort Neg = 54;
        public static ushort Ceq = 55;

        public static ushort Br = 56;
        public static ushort Brtrue = 57;
        public static ushort Brfalse = 58;

        public static ushort Blt = 59;
        public static ushort Blt_Un = 60;
        public static ushort Ble = 61;
        public static ushort Ble_Un = 62;
        public static ushort Bgt = 63;
        public static ushort Bgt_Un = 64;
        public static ushort Beq = 65;
        public static ushort Beq_Un = 66;
        public static ushort Bge = 67;
        public static ushort Bge_Un = 68;
        public static ushort Cgt = 69;
        public static ushort Cgt_Un = 70;
        public static ushort Clt = 71;
        public static ushort Clt_Un = 72;
        public static ushort Bne_Un = 73;

        public static ushort Ldlen = 74;

        public static ushort Stind = 75;
        public static ushort Ldind = 76;

        public static ushort Convert = 77;
        public static ushort Convert_Ovf = 78;
        public static ushort Convert_Ovf_Un = 79;

        public static ushort Leave = 80;
        public static ushort Endfinally = 81;
        public static ushort Endfilter = 82;

        public static ushort Throw = 83;
        public static ushort Ret = 84;
    }
}
