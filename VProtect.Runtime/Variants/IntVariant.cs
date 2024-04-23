using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class IntVariant : StackableVariant
    {
        private int _value;
        public IntVariant(int value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(int);
        }

        public override TypeCode CalcTypeCode()
        {
            return TypeCode.Int32;
        }

        public override BaseVariant Clone()
        {
            return new IntVariant(_value);
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
            _value = System.Convert.ToInt32(value);
        }

        public override bool ToBoolean()
        {
            return _value != 0;
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
            return _value;
        }

        public override long ToInt64()
        {
            return (long)_value;
        }

        public override char ToChar()
        {
            return (char)_value;
        }

        public override byte ToByte()
        {
            return (byte)_value;
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
            return (uint)_value;
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
            return new IntPtr(_value);
        }

        public override UIntPtr ToUIntPtr()
        {
            return new UIntPtr((uint)_value);
        }

        public override BaseVariant ToUnsigned()
        {
            return new UIntVariant((uint)_value);
        }

        public override object Conv_ovf(Type type, bool un)
        {
            if (type == typeof(IntPtr))
            {
                if (IntPtr.Size == 4)
                    return new IntPtr(un ? checked((int)(uint)_value) : _value);
                return new IntPtr(un ? (long)(uint)_value : _value);
            }
            if (type == typeof(UIntPtr))
                return new UIntPtr(un ? (uint)_value : checked((uint)_value));

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return un ? checked((sbyte)(uint)_value) : checked((sbyte)_value);
                case TypeCode.Int16:
                    return un ? checked((short)(uint)_value) : checked((short)_value);
                case TypeCode.Int32:
                    return un ? checked((int)(uint)_value) : _value;
                case TypeCode.Int64:
                    return un ? (long)(uint)_value : _value;
                case TypeCode.Byte:
                    return un ? checked((byte)(uint)_value) : checked((byte)_value);
                case TypeCode.UInt16:
                    return un ? checked((ushort)(uint)_value) : checked((ushort)_value);
                case TypeCode.UInt32:
                    return un ? (uint)_value : checked((uint)_value);
                case TypeCode.UInt64:
                    return un ? (uint)_value : checked((uint)_value);
                case TypeCode.Double:
                    return un ? (double)(uint)_value : (double)_value;
            }

            throw new ArgumentException();
        }
    }
}
