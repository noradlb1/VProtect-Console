using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class IntPtrVariant : StackableVariant
    {
        IntPtr _value;
        private BaseVariant _variant;

        public IntPtrVariant(IntPtr value)
        {
            _value = value;
            _variant = ToVariant(_value);
        }

        private static BaseVariant ToVariant(IntPtr value)
        {
            if (IntPtr.Size == 4)
                return new IntVariant(value.ToInt32());
            else
                return new LongVariant(value.ToInt64());
        }

        public override Type Type()
        {
            return typeof(IntPtr);
        }

        public override TypeCode CalcTypeCode()
        {
            return _variant.CalcTypeCode();
        }

        public override BaseVariant Clone()
        {
            return new IntPtrVariant(_value);
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
            _value = (IntPtr)value;
            _variant = ToVariant(_value);
        }

        public override bool ToBoolean()
        {
            return _value != IntPtr.Zero;
        }

        public override sbyte ToSByte()
        {
            return _variant.ToSByte();
        }

        public override short ToInt16()
        {
            return _variant.ToInt16();
        }

        public override int ToInt32()
        {
            return _variant.ToInt32();
        }

        public override long ToInt64()
        {
            return _variant.ToInt64();
        }

        public override byte ToByte()
        {
            return _variant.ToByte();
        }

        public override ushort ToUInt16()
        {
            return _variant.ToUInt16();
        }

        public override uint ToUInt32()
        {
            return _variant.ToUInt32();
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
            return _value;
        }

        public override UIntPtr ToUIntPtr()
        {
            return _variant.ToUIntPtr();
        }

        public override unsafe void* ToPointer()
        {
            return _value.ToPointer();
        }

        public override object Conv_ovf(Type type, bool un)
        {
            return _variant.Conv_ovf(type, un);
        }
    }
}
