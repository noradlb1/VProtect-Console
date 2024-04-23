using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class EnumVariant : StackableVariant
    {
        private Enum _value;
        private BaseVariant _variant;

        public EnumVariant(Enum value)
        {
            if (value == null)
                throw new ArgumentException();
            _value = value;
            _variant = ToVariant(_value);
        }

        private static BaseVariant ToVariant(Enum value)
        {
            switch (value.GetTypeCode())
            {
                case TypeCode.Byte:
                    return new ByteVariant(System.Convert.ToByte(value));
                case TypeCode.SByte:
                    return new SByteVariant(System.Convert.ToSByte(value));
                case TypeCode.Int16:
                    return new ShortVariant(System.Convert.ToInt16(value));
                case TypeCode.UInt16:
                    return new UShortVariant(System.Convert.ToUInt16(value));
                case TypeCode.Int32:
                    return new IntVariant(System.Convert.ToInt32(value));
                case TypeCode.UInt32:
                    return new UIntVariant(System.Convert.ToUInt32(value));
                case TypeCode.Int64:
                    return new LongVariant(System.Convert.ToInt64(value));
                case TypeCode.UInt64:
                    return new ULongVariant(System.Convert.ToUInt64(value));
                case TypeCode.Single:
                    return new SingleVariant(System.Convert.ToSingle(value));
                case TypeCode.Double:
                    return new DoubleVariant(System.Convert.ToDouble(value));
                default:
                    throw new InvalidOperationException();
            }
        }

        public override BaseVariant ToUnsigned()
        {
            return _variant.ToUnsigned();
        }

        public override Type Type()
        {
            return _value.GetType();
        }

        public override TypeCode CalcTypeCode()
        {
            return _variant.CalcTypeCode();
        }

        public override BaseVariant Clone()
        {
            return new EnumVariant(_value);
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
            if (value == null)
                throw new ArgumentException();

            _value = (Enum)value;
            _variant = ToVariant(_value);
        }

        public override byte ToByte()
        {
            return _variant.ToByte();
        }

        public override sbyte ToSByte()
        {
            return _variant.ToSByte();
        }

        public override short ToInt16()
        {
            return _variant.ToInt16();
        }

        public override ushort ToUInt16()
        {
            return _variant.ToUInt16();
        }

        public override int ToInt32()
        {
            return _variant.ToInt32();
        }

        public override uint ToUInt32()
        {
            return _variant.ToUInt32();
        }

        public override long ToInt64()
        {
            return _variant.ToInt64();
        }

        public override ulong ToUInt64()
        {
            return _variant.ToUInt64();
        }

        public override float ToSingle()
        {
            return _variant.ToSingle();
        }
        public override double ToDouble()
        {
            return _variant.ToDouble();
        }

        public override IntPtr ToIntPtr()
        {
            return new IntPtr(IntPtr.Size == 4 ? ToInt32() : ToInt64());
        }

        public override UIntPtr ToUIntPtr()
        {
            return new UIntPtr(IntPtr.Size == 4 ? ToUInt32() : ToUInt64());
        }

        public override object Conv_ovf(Type type, bool un)
        {
            return _variant.Conv_ovf(type, un);
        }
    }
}
