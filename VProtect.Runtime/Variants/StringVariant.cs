using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class StringVariant : StackableVariant
    {
        private string _value;

        public StringVariant(string value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(string);
        }

        public override TypeCode CalcTypeCode()
        {
            return TypeCode.Object;
        }

        public override BaseVariant Clone()
        {
            return new StringVariant(_value);
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
            _value = (value != null) ? System.Convert.ToString(value) : null;
        }

        public override bool ToBoolean()
        {
            return _value != null;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
