using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class ULongVariant : BaseVariant
    {
        private ulong _value;

        public ULongVariant(ulong value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(ulong);
        }

        public override BaseVariant Clone()
        {
            return new ULongVariant(_value);
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
            _value = System.Convert.ToUInt64(value);
        }

        public override StackableVariant ToStack()
        {
            return new LongVariant(ToInt64());
        }

        public override sbyte ToSByte()
        {
            return (sbyte)_value;
        }

        public override byte ToByte()
        {
            return (byte)_value;
        }

        public override short ToInt16()
        {
            return (short)_value;
        }

        public override ushort ToUInt16()
        {
            return (ushort)_value;
        }

        public override int ToInt32()
        {
            return (int)_value;
        }

        public override uint ToUInt32()
        {
            return (uint)_value;
        }

        public override long ToInt64()
        {
            return (long)_value;
        }

        public override ulong ToUInt64()
        {
            return _value;
        }
    }
}
