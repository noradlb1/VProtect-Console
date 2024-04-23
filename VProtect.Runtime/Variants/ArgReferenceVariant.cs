using System;

namespace VProtect.Runtime.Variants
{
    internal sealed class ArgReferenceVariant : StackableVariant
    {
        private BaseVariant _variable;
        private BaseVariant _arg;

        public ArgReferenceVariant(BaseVariant variable, BaseVariant arg)
        {
            _variable = variable;
            _arg = arg;
        }

        public override Type Type()
        {
            return _variable.Type();
        }

        public override BaseVariant Clone()
        {
            return new ArgReferenceVariant(_variable, _arg);
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
            _arg.SetValue(_variable.Value());
        }

        public override bool ToBoolean()
        {
            return _variable != null;
        }
    }
}
