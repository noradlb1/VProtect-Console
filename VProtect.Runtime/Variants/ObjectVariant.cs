using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class ObjectVariant : StackableVariant
    {
        private object _value;

        public ObjectVariant(object value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(object);
        }

        public override TypeCode CalcTypeCode()
        {
            return TypeCode.Object;
        }

        public override BaseVariant Clone()
        {
            return new ObjectVariant(_value);
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
            _value = value;
        }

        public override bool ToBoolean()
        {
            return _value != null;
        }
    }
}
