using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class UShortVariant : BaseVariant
    {
        private ushort _value;

        public UShortVariant(ushort value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(ushort);
        }

        public override BaseVariant Clone()
        {
            return new UShortVariant(_value);
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
            _value = System.Convert.ToUInt16(value);
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
            return (byte)_value;
        }

        public override short ToInt16()
        {
            return (short)_value;
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
