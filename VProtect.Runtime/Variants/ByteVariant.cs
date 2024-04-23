using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class ByteVariant : BaseVariant
    {
        private byte _value;

        public ByteVariant(byte value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(byte);
        }

        public override BaseVariant Clone()
        {
            return new ByteVariant(_value);
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
            _value = System.Convert.ToByte(value);
        }

        public override StackableVariant ToStack()
        {
            return new IntVariant(ToInt32());
        }

        public override sbyte ToSByte()
        {
            return (sbyte)_value;
        }

        public override byte ToByte()
        {
            return _value;
        }

        public override short ToInt16()
        {
            return _value;
        }

        public override ushort ToUInt16()
        {
            return _value;
        }

        public override int ToInt32()
        {
            return _value;
        }
        public override uint ToUInt32()
        {
            return _value;
        }
    }
}
