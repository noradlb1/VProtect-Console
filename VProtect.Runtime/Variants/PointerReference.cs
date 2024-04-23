using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace VProtect.Runtime.Variants
{
    internal sealed class PointerReference : StackableVariant
    {
        private IntPtr _value;
        private Type _type;

        public PointerReference(IntPtr value, Type type)
        {
            _value = value;
            _type = type;
        }

        public override Type Type()
        {
            return typeof(Pointer);
        }

        public override TypeCode CalcTypeCode()
        {
            return TypeCode.Empty;
        }

        public override BaseVariant Clone()
        {
            return new PointerReference(_value, _type);
        }

        public override object Value()
        {
            if (_type.IsValueType)
                return Marshal.PtrToStructure(_value, _type);

            throw new InvalidOperationException();
        }

        public override bool IsReference()
        {
            return true;
        }

        public override void SetValue(object value)
        {
            if (value == null)
                throw new ArgumentException();

            if (_type.IsValueType)
            {
                Marshal.StructureToPtr(value, _value, false);
                return;
            }

            switch (System.Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.SByte:
                    Marshal.WriteByte(_value, (byte)System.Convert.ToSByte(value));
                    break;
                case TypeCode.Byte:
                    Marshal.WriteByte(_value, System.Convert.ToByte(value));
                    break;
                case TypeCode.Char:
                    Marshal.WriteInt16(_value, System.Convert.ToChar(value));
                    break;
                case TypeCode.Int16:
                    Marshal.WriteInt16(_value, System.Convert.ToInt16(value));
                    break;
                case TypeCode.UInt16:
                    Marshal.WriteInt16(_value, (short)System.Convert.ToUInt16(value));
                    break;
                case TypeCode.Int32:
                    Marshal.WriteInt32(_value, System.Convert.ToInt32(value));
                    break;
                case TypeCode.UInt32:
                    Marshal.WriteInt32(_value, (int)System.Convert.ToUInt32(value));
                    break;
                case TypeCode.Int64:
                    Marshal.WriteInt64(_value, System.Convert.ToInt64(value));
                    break;
                case TypeCode.UInt64:
                    Marshal.WriteInt64(_value, (long)System.Convert.ToUInt64(value));
                    break;
                case TypeCode.Single:
                    Marshal.WriteInt32(_value, BitConverter.ToInt32(BitConverter.GetBytes(System.Convert.ToSingle(value)), 0));
                    break;
                case TypeCode.Double:
                    Marshal.WriteInt64(_value, BitConverter.ToInt64(BitConverter.GetBytes(System.Convert.ToDouble(value)), 0));
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public override sbyte ToSByte()
        {
            return (sbyte)Marshal.ReadByte(_value);
        }

        public override short ToInt16()
        {
            return Marshal.ReadInt16(_value);
        }

        public override int ToInt32()
        {
            return Marshal.ReadInt32(_value);
        }

        public override long ToInt64()
        {
            return Marshal.ReadInt64(_value);
        }

        public override char ToChar()
        {
            return (char)Marshal.ReadInt16(_value);
        }

        public override byte ToByte()
        {
            return Marshal.ReadByte(_value);
        }
        public override ushort ToUInt16()
        {
            return (ushort)Marshal.ReadInt16(_value);
        }

        public override uint ToUInt32()
        {
            return (uint)Marshal.ReadInt32(_value);
        }

        public override ulong ToUInt64()
        {
            return (ulong)Marshal.ReadInt64(_value);
        }

        public override float ToSingle()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(Marshal.ReadInt32(_value)), 0);
        }

        public override double ToDouble()
        {
            return BitConverter.ToDouble(BitConverter.GetBytes(Marshal.ReadInt64(_value)), 0);
        }

        public override IntPtr ToIntPtr()
        {
            return new IntPtr(IntPtr.Size == 4 ? Marshal.ReadInt32(_value) : Marshal.ReadInt64(_value));
        }

        public override UIntPtr ToUIntPtr()
        {
            return new UIntPtr(IntPtr.Size == 4 ? (uint)Marshal.ReadInt32(_value) : (ulong)Marshal.ReadInt64(_value));
        }
    }
}
