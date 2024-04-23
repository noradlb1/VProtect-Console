using System;
using System.Reflection;

namespace VProtect.Runtime.Variants
{
    internal sealed class MethodVariant : StackableVariant
    {
        private MethodBase _value;

        public MethodVariant(MethodBase value)
        {
            _value = value;
        }

        public override Type Type()
        {
            return typeof(MethodBase);
        }

        public override BaseVariant Clone()
        {
            return new MethodVariant(_value);
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
            _value = (MethodBase)value;
        }

        public override bool ToBoolean()
        {
            return _value != null;
        }

        public override IntPtr ToIntPtr()
        {
            return _value.MethodHandle.GetFunctionPointer();
        }
    }
}
