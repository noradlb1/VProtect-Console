using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class SByteVariant : BaseVariant
    {
        private sbyte _value;

        public SByteVariant(sbyte value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(sbyte);
        }

        public override BaseVariant Clone()
        {
            return new SByteVariant(_value);
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
            _value = System.Convert.ToSByte(value);
        }

        public override StackableVariant ToStack()
        {
            return new IntVariant(ToInt32());
        }

        public override sbyte ToSByte()
        {
            return _value;
        }

        public override byte ToByte()
        {
            return (byte)_value;
        }

        public override short ToInt16()
        {
            return _value;
        }

        public override ushort ToUInt16()
        {
            return (ushort)_value;
        }

        public override int ToInt32()
        {
            return _value;
        }

        public override uint ToUInt32()
        {
            return (uint)_value;
        }
    }
}
