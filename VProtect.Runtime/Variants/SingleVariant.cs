using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class SingleVariant : StackableVariant
    {
        private float _value;

        public SingleVariant(float value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(float);
        }

        public override TypeCode CalcTypeCode()
        {
            return TypeCode.Single;
        }

        public override BaseVariant Clone()
        {
            return new SingleVariant(_value);
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
            _value = System.Convert.ToSingle(value);
        }

        public override bool ToBoolean()
        {
            return System.Convert.ToBoolean(_value);
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
            return (ulong)_value;
        }

        public override float ToSingle()
        {
            return _value;
        }

        public override double ToDouble()
        {
            return _value;
        }

        public override IntPtr ToIntPtr()
        {
            return new IntPtr(IntPtr.Size == 4 ? (int)_value : (long)_value);
        }

        public override UIntPtr ToUIntPtr()
        {
            return new UIntPtr(IntPtr.Size == 4 ? (uint)_value : (ulong)_value);
        }

        public override object Conv_ovf(Type type, bool un)
        {
            if (type == typeof(IntPtr))
                return new IntPtr(checked((long)_value));
            if (type == typeof(UIntPtr))
                return new UIntPtr(checked((ulong)_value));

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return un ? checked((sbyte)(uint)_value) : checked((sbyte)_value);
                case TypeCode.Int16:
                    return un ? checked((short)(uint)_value) : checked((short)_value);
                case TypeCode.Int32:
                    return checked((int)_value);
                case TypeCode.Byte:
                    return checked((byte)_value);
                case TypeCode.UInt16:
                    return checked((ushort)_value);
                case TypeCode.UInt32:
                    return checked((uint)_value);
                case TypeCode.UInt64:
                    return checked((ulong)_value);
            }

            throw new ArgumentException();
        }
    }
}
