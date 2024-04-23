using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class ArrayVariant : StackableVariant
    {
        private Array _value;

        public ArrayVariant(Array value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(Array);
        }

        public override BaseVariant Clone()
        {
            return new ArrayVariant(_value);
        }

        public override bool IsReference()
        {
            return false;
        }

        public override object Value()
        {
            return _value;
        }

        public override void SetValue(object value)
        {
            _value = (Array)value;
        }

        public override bool ToBoolean()
        {
            return _value != null;
        }
    }
}
