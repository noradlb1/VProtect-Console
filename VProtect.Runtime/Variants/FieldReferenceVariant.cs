using System;
using System.Reflection;

namespace VProtect.Runtime.Variants
{
    internal sealed class FieldReferenceVariant : StackableVariant
    {
        private FieldInfo _field;
        private BaseVariant _variable;

        public FieldReferenceVariant(FieldInfo field, BaseVariant variable)
        {
            _field = field;
            _variable = variable;
        }

        public override Type Type()
        {
            return _field.FieldType;
        }

        public override BaseVariant Clone()
        {
            return new FieldReferenceVariant(_field, _variable);
        }

        public override bool IsReference()
        {
            return true;
        }

        public override object Value()
        {
            return _field.GetValue(_variable.Value());
        }

        public override void SetValue(object value)
        {
            _variable.SetFieldValue(_field, value);
        }
    }
}
