using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class BoolVariant : BaseVariant
    {
        private bool _value;

        public BoolVariant(bool value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(bool);
        }

        public override BaseVariant Clone()
        {
            return new BoolVariant(_value);
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
            _value = System.Convert.ToBoolean(value);
        }

        public override StackableVariant ToStack()
        {
            return new IntVariant(ToInt32());
        }

        public override int ToInt32()
        {
            return _value ? 1 : 0;
        }
    }
}
