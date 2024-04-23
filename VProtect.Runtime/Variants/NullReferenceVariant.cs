using System;
using System.Reflection;

namespace VProtect.Runtime.Variants
{
    internal sealed class NullReferenceVariant : StackableVariant
    {
        private Type _type;

        public NullReferenceVariant(Type type)
        {
            _type = type;
        }

        public override Type Type()
        {
            return _type;
        }

        public override BaseVariant Clone()
        {
            return new NullReferenceVariant(_type);
        }

        public override bool IsReference()
        {
            return true;
        }

        public override object Value()
        {
            throw new InvalidOperationException();
        }

        public override void SetValue(object value)
        {
            throw new InvalidOperationException();
        }

        public override void SetFieldValue(FieldInfo field, object value)
        {
            throw new InvalidOperationException();
        }
    }
}
