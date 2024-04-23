using System;

namespace VProtect.Runtime.Variants
{
    internal abstract class StackableVariant : BaseVariant
    {
        public override StackableVariant ToStack()
        {
            return this;
        }

        public override TypeCode CalcTypeCode()
        {
            return TypeCode.Empty;
        }
    }
}
