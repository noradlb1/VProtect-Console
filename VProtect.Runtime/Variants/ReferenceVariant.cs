using System;
using System.Reflection;

namespace VProtect.Runtime.Variants
{
    internal sealed class ReferenceVariant : StackableVariant
    {
        private BaseVariant _variable;

        public ReferenceVariant(BaseVariant variable)
        {
            _variable = variable;
        }

        public override Type Type()
        {
            return _variable.Type();
        }

        public override BaseVariant Clone()
        {
            return new ReferenceVariant(_variable);
        }

        public override bool IsReference()
        {
            return true;
        }

        public override object Value()
        {
            return _variable.Value();
        }

        public override void SetValue(object value)
        {
            _variable.SetValue(value);
        }

        public override bool ToBoolean()
        {
            return _variable != null;
        }

        public override void SetFieldValue(FieldInfo field, object value)
        {
            _variable.SetFieldValue(field, value);
        }
    }
}
