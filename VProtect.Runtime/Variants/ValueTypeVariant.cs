using System;
using System.Reflection;

namespace VProtect.Runtime.Variants
{
    internal sealed class ValueTypeVariant : StackableVariant
    {
        private object _value;

        public ValueTypeVariant(object value)
        {
            if (value != null && !(value is ValueType))
                throw new ArgumentException();
            _value = value;
        }

        public override Type Type()
        {
            return typeof(ValueType);
        }

        public override BaseVariant Clone()
        {
            object value;
            if (_value == null)
            {
                value = null;
            }
            else
            {
                var type = _value.GetType();
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                value = Activator.CreateInstance(type);
                foreach (var field in fields)
                {
                    field.SetValue(value, field.GetValue(_value));
                }
            }

            return new ValueTypeVariant(value);
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
            if (value != null && !(value is ValueType))
                throw new ArgumentException();

            _value = value;
        }
    }
}
