using System;
using System.Reflection;
using System.Runtime.InteropServices;

using VProtect.Runtime.Execution;
using VProtect.Runtime.Variants.VariantsTypes;

namespace VProtect.Runtime.Variants
{
    internal sealed class LocalVariant : StackableVariant
    {
        private VMContext _ctx;

        private object _value;
        private Type _type;
        private int _localIndex;
        private bool _islocalA;

        public LocalVariant(VMContext ctx, object value, Type type, int localIndex, bool islocalA)
        {
            _ctx = ctx;

            if (type.IsValueType)
                _value = value == null ? Activator.CreateInstance(type) : value;
            else if (type.IsPointer)
            {
                GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
                _value = handle.AddrOfPinnedObject();
                handle.Free();
            }
            else
                _value = value;

            _type = type;
            _localIndex = localIndex;
            _islocalA = islocalA;
        }

        public override bool IsReference()
        {
            return _islocalA;
        }

        public override Type Type()
        {
            return typeof(VLocal);
        }

        public override TypeCode CalcTypeCode()
        {
            return System.Type.GetTypeCode(_type);
        }

        public override BaseVariant Clone()
        {
            return new LocalVariant(_ctx, _value, _type, _localIndex, _islocalA);
        }

        public override object Value()
        {
            return _value;
        }

        public int Index()
        {
            return _localIndex;
        }

        public override void SetValue(object value)
        {
            _value = value;
            _type = value.GetType();

            _ctx.VMethodInfo.Locals[_localIndex] = value;
        }

        public void SetLocalValue(VMContext ctx, object value)
        {
            _value = value;
            _type = value.GetType();

            ctx.VMethodInfo.Locals[_localIndex] = value;
        }

        public override bool ToBoolean()
        {
            return System.Convert.ToBoolean(_value);
        }

        public override unsafe byte ToByte()
        {
            if (_value.GetType() == typeof(UIntPtr))
                return unchecked((byte)((UIntPtr)_value).ToUInt32());
            else if (_value.GetType() == typeof(IntPtr))
                return unchecked((byte)((IntPtr)_value).ToInt32());

            return System.Convert.ToByte(_value);
        }

        public override char ToChar()
        {
            return System.Convert.ToChar(_value);
        }

        public override sbyte ToSByte()
        {
            return System.Convert.ToSByte(_value);
        }

        public override short ToInt16()
        {
            if (_value.GetType() == typeof(UIntPtr))
                return unchecked((short)((UIntPtr)_value).ToUInt32());
            else if (_value.GetType() == typeof(IntPtr))
                return unchecked((short)((IntPtr)_value).ToInt32());

            return System.Convert.ToInt16(_value);
        }

        public override int ToInt32()
        {
            if (_value.GetType() == typeof(UIntPtr))
                return unchecked((int)((UIntPtr)_value).ToUInt32());
            else if (_value.GetType() == typeof(IntPtr))
                return ((IntPtr)_value).ToInt32();

            return System.Convert.ToInt32(_value);
        }

        public override long ToInt64()
        {
            if (_value.GetType() == typeof(UIntPtr))
                return unchecked((long)((UIntPtr)_value).ToUInt64());
            else if (_value.GetType() == typeof(IntPtr))
                return ((IntPtr)_value).ToInt64();

            return System.Convert.ToInt64(_value);
        }

        public override ushort ToUInt16()
        {
            if (_value.GetType() == typeof(UIntPtr))
                return unchecked((ushort)((UIntPtr)_value).ToUInt64());
            else if (_value.GetType() == typeof(IntPtr))
                return unchecked((ushort)((IntPtr)_value).ToInt64());

            return System.Convert.ToUInt16(_value);
        }

        public override uint ToUInt32()
        {
            if (_value.GetType() == typeof(UIntPtr))
                return ((UIntPtr)_value).ToUInt32();
            else if (_value.GetType() == typeof(IntPtr))
                return unchecked((uint)((IntPtr)_value).ToInt32());

            return System.Convert.ToUInt32(_value);
        }

        public override ulong ToUInt64()
        {
            if (_value.GetType() == typeof(UIntPtr))
                return ((UIntPtr)_value).ToUInt64();
            else if (_value.GetType() == typeof(IntPtr))
                return unchecked((ulong)((IntPtr)_value).ToInt64());

            return System.Convert.ToUInt64(_value);
        }

        public override float ToSingle()
        {
            return System.Convert.ToSingle(_value);
        }

        public override double ToDouble()
        {
            return System.Convert.ToDouble(_value);
        }

        public override IntPtr ToIntPtr()
        {
            if (((_value != null) ? _value.GetType() : _type) == typeof(IntPtr))
                return (IntPtr)_value;

            return new IntPtr(IntPtr.Size == 4 ? ToInt32() : ToInt64());
        }

        public override UIntPtr ToUIntPtr()
        {
            if (((_value != null) ? _value.GetType() : _type) == typeof(UIntPtr))
                return (UIntPtr)_value;

            return new UIntPtr(IntPtr.Size == 4 ? ToUInt32() : ToUInt64());
        }

        public override unsafe void* ToPointer()
        {
            return ((IntPtr)_value).ToPointer();
        }

        public override BaseVariant ToUnsigned()
        {
            if (_value.GetType() == typeof(int))
                return new UIntVariant((uint)_value);
            else if (_value.GetType() == typeof(long))
                return new ULongVariant((ulong)_value);
            else
                return null;
        }

        public override object Conv_ovf(Type type, bool un)
        {
            if (_value.GetType() == typeof(int))
                return Conv_ovf_I4((int)_value, type, un);
            else if (_value.GetType() == typeof(long))
                return Conv_ovf_I8((long)_value, type, un);
            else
                return null;
        }

        private object Conv_ovf_I8(long value, Type type, bool un)
        {
            if (type == typeof(IntPtr))
                return new IntPtr(un ? checked((long)(ulong)value) : value);
            if (type == typeof(UIntPtr))
                return new UIntPtr(un ? (ulong)value : checked((ulong)value));

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return un ? checked((sbyte)(ulong)value) : checked((sbyte)value);
                case TypeCode.Int16:
                    return un ? checked((short)(ulong)value) : checked((short)value);
                case TypeCode.Int32:
                    return un ? checked((int)(ulong)value) : checked((int)value);
                case TypeCode.Int64:
                    return un ? checked((long)(ulong)value) : value;
                case TypeCode.Byte:
                    return un ? checked((byte)(ulong)value) : checked((byte)value);
                case TypeCode.UInt16:
                    return un ? checked((ushort)(uint)value) : checked((ushort)value);
                case TypeCode.UInt32:
                    return un ? checked((uint)(ulong)value) : checked((uint)value);
                case TypeCode.UInt64:
                    return un ? (ulong)value : checked((ulong)value);
                case TypeCode.Double:
                    return un ? (double)(ulong)value : (double)value;
            }

            throw new ArgumentException();
        }

        private object Conv_ovf_I4(int value, Type type, bool un)
        {
            if (type == typeof(IntPtr))
            {
                if (IntPtr.Size == 4)
                    return new IntPtr(un ? checked((int)(uint)value) : value);
                return new IntPtr(un ? (long)(uint)value : value);
            }
            if (type == typeof(UIntPtr))
                return new UIntPtr(un ? (uint)value : checked((uint)value));

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return un ? checked((sbyte)(uint)value) : checked((sbyte)value);
                case TypeCode.Int16:
                    return un ? checked((short)(uint)value) : checked((short)value);
                case TypeCode.Int32:
                    return un ? checked((int)(uint)value) : value;
                case TypeCode.Int64:
                    return un ? (long)(uint)value : value;
                case TypeCode.Byte:
                    return un ? checked((byte)(uint)value) : checked((byte)value);
                case TypeCode.UInt16:
                    return un ? checked((ushort)(uint)value) : checked((ushort)value);
                case TypeCode.UInt32:
                    return un ? (uint)value : checked((uint)value);
                case TypeCode.UInt64:
                    return un ? (uint)value : checked((uint)value);
                case TypeCode.Double:
                    return un ? (double)(uint)value : (double)value;
            }

            throw new ArgumentException();
        }
    }
}
