using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class LongVariant : StackableVariant
    {
        private long _value;
        public LongVariant(long value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(long);
        }

        public override TypeCode CalcTypeCode()
        {
            return TypeCode.Int64;
        }

        public override BaseVariant ToUnsigned()
        {
            return new ULongVariant((ulong)_value);
        }

        public override BaseVariant Clone()
        {
            return new LongVariant(_value);
        }

        public override object Value()
        {
            return _value;
        }

        public override bool IsReference()
        {
            return false;
        }

        public override void SetValue(object value)
        {
            _value = System.Convert.ToInt64(value);
        }

        public override bool ToBoolean()
        {
            return _value != 0;
        }

        public override char ToChar()
        {
            return (char)_value;
        }

        public override byte ToByte()
        {
            return (byte)_value;
        }

        public override sbyte ToSByte()
        {
            return (sbyte)_value;
        }

        public override short ToInt16()
        {
            return (short)_value;
        }

        public override int ToInt32()
        {
            return (int)_value;
        }

        public override long ToInt64()
        {
            return _value;
        }

        public override ushort ToUInt16()
        {
            return (ushort)_value;
        }

        public override uint ToUInt32()
        {
            return (uint)_value;
        }

        public override ulong ToUInt64()
        {
            return (ulong)_value;
        }

        public override float ToSingle()
        {
            return _value;
        }

        public override double ToDouble()
        {
            return (double)_value;
        }

        public override IntPtr ToIntPtr()
        {
            return new IntPtr(IntPtr.Size == 4 ? (int)_value : _value);
        }

        public override UIntPtr ToUIntPtr()
        {
            return new UIntPtr(UIntPtr.Size == 4 ? (uint)_value : (ulong)_value);
        }

        public override object Conv_ovf(Type type, bool un)
        {
            if (type == typeof(IntPtr))
                return new IntPtr(un ? checked((long)(ulong)_value) : _value);
            if (type == typeof(UIntPtr))
                return new UIntPtr(un ? (ulong)_value : checked((ulong)_value));

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return un ? checked((sbyte)(ulong)_value) : checked((sbyte)_value);
                case TypeCode.Int16:
                    return un ? checked((short)(ulong)_value) : checked((short)_value);
                case TypeCode.Int32:
                    return un ? checked((int)(ulong)_value) : checked((int)_value);
                case TypeCode.Int64:
                    return un ? checked((long)(ulong)_value) : _value;
                case TypeCode.Byte:
                    return un ? checked((byte)(ulong)_value) : checked((byte)_value);
                case TypeCode.UInt16:
                    return un ? checked((ushort)(uint)_value) : checked((ushort)_value);
                case TypeCode.UInt32:
                    return un ? checked((uint)(ulong)_value) : checked((uint)_value);
                case TypeCode.UInt64:
                    return un ? (ulong)_value : checked((ulong)_value);
                case TypeCode.Double:
                    return un ? (double)(ulong)_value : (double)_value;
            }

            throw new ArgumentException();
        }
    }
}
