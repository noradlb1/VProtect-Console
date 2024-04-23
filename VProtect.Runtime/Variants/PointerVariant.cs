using System;
using System.Reflection;

namespace VProtect.Runtime.Variants
{
    internal sealed class PointerVariant : StackableVariant
    {
        private object _value;
        private Type _type;
        private BaseVariant _variant;

        public PointerVariant(object value, Type type)
        {
            _value = value;
            _type = type;
            _variant = ToVariant(value);
        }

        private static BaseVariant ToVariant(object value)
        {
            unsafe
            {
                IntPtr ptr = value == null ? IntPtr.Zero : new IntPtr(Pointer.Unbox(value));
                if (IntPtr.Size == 4)
                    return new IntVariant(ptr.ToInt32());
                else
                    return new LongVariant(ptr.ToInt64());
            }
        }

        public override Type Type()
        {
            return _type;
        }

        public override TypeCode CalcTypeCode()
        {
            return (IntPtr.Size == 4) ? TypeCode.UInt32 : TypeCode.UInt64;
        }

        public override BaseVariant Clone()
        {
            return new PointerVariant(_value, _type);
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
            _variant = ToVariant(value);
        }

        public override bool ToBoolean()
        {
            return _value != null;
        }

        public override sbyte ToSByte()
        {
            return _variant.ToSByte();
        }

        public override short ToInt16()
        {
            return _variant.ToInt16();
        }

        public override int ToInt32()
        {
            return _variant.ToInt32();
        }

        public override long ToInt64()
        {
            return _variant.ToInt64();
        }

        public override byte ToByte()
        {
            return _variant.ToByte();
        }

        public override ushort ToUInt16()
        {
            return _variant.ToUInt16();
        }

        public override uint ToUInt32()
        {
            return _variant.ToUInt32();
        }

        public override ulong ToUInt64()
        {
            return _variant.ToUInt64();
        }

        public override float ToSingle()
        {
            return _variant.ToSingle();
        }

        public override double ToDouble()
        {
            return _variant.ToDouble();
        }

        public override IntPtr ToIntPtr()
        {
            return _variant.ToIntPtr();
        }

        public override UIntPtr ToUIntPtr()
        {
            return _variant.ToUIntPtr();
        }

        public override unsafe void* ToPointer()
        {
            return Pointer.Unbox(_value);
        }

        public override object Conv_ovf(Type type, bool un)
        {
            return _variant.Conv_ovf(type, un);
        }
    }
}
