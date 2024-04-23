using System;
using System.Reflection;

namespace VProtect.Runtime.Variants
{
    internal abstract class BaseVariant
    {
        public abstract BaseVariant Clone();
        public abstract bool IsReference();
        public abstract object Value();
        public abstract void SetValue(object value);

        public virtual void SetFieldValue(FieldInfo field, object value)
        {
            var obj = Value();
            field.SetValue(obj, value);
        }

        public virtual StackableVariant ToStack()
        {
            throw new InvalidOperationException();
        }

        public virtual BaseVariant ToUnsigned()
        {
            return this;
        }

        public virtual Type Type()
        {
            throw new InvalidOperationException();
        }

        public virtual TypeCode CalcTypeCode()
        {
            throw new InvalidOperationException();
        }

        public virtual bool ToBoolean()
        {
            return System.Convert.ToBoolean(Value());
        }
        public virtual sbyte ToSByte()
        {
            return System.Convert.ToSByte(Value());
        }
        public virtual short ToInt16()
        {
            return System.Convert.ToInt16(Value());
        }
        public virtual int ToInt32()
        {
            return System.Convert.ToInt32(Value());
        }
        public virtual long ToInt64()
        {
            return System.Convert.ToInt64(Value());
        }
        public virtual char ToChar()
        {
            return System.Convert.ToChar(Value());
        }
        public virtual byte ToByte()
        {
            return System.Convert.ToByte(Value());
        }
        public virtual ushort ToUInt16()
        {
            return System.Convert.ToUInt16(Value());
        }
        public virtual uint ToUInt32()
        {
            return System.Convert.ToUInt32(Value());
        }
        public virtual ulong ToUInt64()
        {
            return System.Convert.ToUInt64(Value());
        }
        public virtual float ToSingle()
        {
            return System.Convert.ToSingle(Value());
        }
        public virtual double ToDouble()
        {
            return System.Convert.ToDouble(Value());
        }

        public override string ToString()
        {
            var v = Value();
            return v != null ? System.Convert.ToString(v) : null;
        }

        public virtual IntPtr ToIntPtr()
        {
            var value = Value();
            if (((value != null) ? value.GetType() : null) == typeof(IntPtr))
                return (IntPtr)value;

            throw new InvalidOperationException();
        }

        public virtual UIntPtr ToUIntPtr()
        {
            var value = Value();
            if (((value != null) ? value.GetType() : null) == typeof(UIntPtr))
                return (UIntPtr)value;

            throw new InvalidOperationException();
        }

        public virtual unsafe void* ToPointer()
        {
            throw new InvalidOperationException();
        }

        public virtual object Conv_ovf(Type type, bool un)
        {
            throw new InvalidOperationException();
        }

        public BaseVariant Convert(Type t)
        {
            if (t.IsEnum)
            {
                var obj = Value();
                if (obj != null && !(obj is Enum))
                    obj = Enum.ToObject(t, obj);

                return new EnumVariant((Enum)obj);
            }

            switch (System.Type.GetTypeCode(t))
            {
                case TypeCode.Boolean:
                    return new BoolVariant(ToBoolean());
                case TypeCode.Char:
                    return new CharVariant(ToChar());
                case TypeCode.SByte:
                    return new SByteVariant(ToSByte());
                case TypeCode.Byte:
                    return new ByteVariant(ToByte());
                case TypeCode.Int16:
                    return new ShortVariant(ToInt16());
                case TypeCode.UInt16:
                    return new UShortVariant(ToUInt16());
                case TypeCode.Int32:
                    return new IntVariant(ToInt32());
                case TypeCode.UInt32:
                    return new UIntVariant(ToUInt32());
                case TypeCode.Int64:
                    return new LongVariant(ToInt64());
                case TypeCode.UInt64:
                    return new ULongVariant(ToUInt64());
                case TypeCode.Single:
                    return new SingleVariant(ToSingle());
                case TypeCode.Double:
                    return new DoubleVariant(ToDouble());
                case TypeCode.String:
                    return new StringVariant(ToString());
                default:
                    if (t == typeof(IntPtr))
                        return new IntPtrVariant(ToIntPtr());
                    else if (t == typeof(UIntPtr))
                        return new UIntPtrVariant(ToUIntPtr());
                    else if (t.IsValueType)
                        return new ValueTypeVariant(Value());
                    else if (t.IsArray)
                        return new ArrayVariant((Array)Value());
                    else if (t.IsPointer)
                    {
                        unsafe
                        {
                            return new PointerVariant(Pointer.Box(ToIntPtr().ToPointer(), t), t);
                        }
                    }
                    else
                        return new ObjectVariant(Value());
            }
        }

        public static BaseVariant Convert(object obj, Type t)
        {
            BaseVariant v = obj as BaseVariant;

            if (t.IsEnum)
            {
                if (v != null)
                    obj = v.Value();
                if (obj != null && !(obj is Enum))
                    obj = Enum.ToObject(t, obj);
                return new EnumVariant(obj == null ? (Enum)Activator.CreateInstance(t) : (Enum)obj);
            }

            switch (System.Type.GetTypeCode(t))
            {
                case TypeCode.Boolean:
                    return new BoolVariant(v != null ? v.ToBoolean() : System.Convert.ToBoolean(obj));
                case TypeCode.Char:
                    return new CharVariant(v != null ? v.ToChar() : System.Convert.ToChar(obj));
                case TypeCode.SByte:
                    return new SByteVariant(v != null ? v.ToSByte() : System.Convert.ToSByte(obj));
                case TypeCode.Byte:
                    return new ByteVariant(v != null ? v.ToByte() : System.Convert.ToByte(obj));
                case TypeCode.Int16:
                    return new ShortVariant(v != null ? v.ToInt16() : System.Convert.ToInt16(obj));
                case TypeCode.UInt16:
                    return new UShortVariant(v != null ? v.ToUInt16() : System.Convert.ToUInt16(obj));
                case TypeCode.Int32:
                    return new IntVariant(v != null ? v.ToInt32() : System.Convert.ToInt32(obj));
                case TypeCode.UInt32:
                    return new UIntVariant(v != null ? v.ToUInt32() : System.Convert.ToUInt32(obj));
                case TypeCode.Int64:
                    return new LongVariant(v != null ? v.ToInt64() : System.Convert.ToInt64(obj));
                case TypeCode.UInt64:
                    return new ULongVariant(v != null ? v.ToUInt64() : System.Convert.ToUInt64(obj));
                case TypeCode.Single:
                    return new SingleVariant(v != null ? v.ToSingle() : System.Convert.ToSingle(obj));
                case TypeCode.Double:
                    return new DoubleVariant(v != null ? v.ToDouble() : System.Convert.ToDouble(obj));
                case TypeCode.String:
                    return new StringVariant(v != null ? v.ToString() : (string)obj);
                default:
                    if (t == typeof(IntPtr))
                    {
                        if (v != null)
                            return new IntPtrVariant(v.ToIntPtr());
                        return new IntPtrVariant(obj != null ? (IntPtr)obj : IntPtr.Zero);
                    }
                    else if (t == typeof(UIntPtr))
                    {
                        if (v != null)
                            return new UIntPtrVariant(v.ToUIntPtr());
                        return new UIntPtrVariant(obj != null ? (UIntPtr)obj : UIntPtr.Zero);
                    }
                    else if (t.IsValueType)
                    {
                        if (v != null)
                            return new ValueTypeVariant(v.Value());
                        return new ValueTypeVariant(obj == null ? Activator.CreateInstance(t) : obj);
                    }
                    else if (t.IsArray)
                        return new ArrayVariant(v != null ? (Array)v.Value() : (Array)obj);
                    else if (t.IsPointer)
                    {
                        unsafe
                        {
                            if (v != null)
                                return new PointerVariant(Pointer.Box(v.ToPointer(), t), t);
                            return new PointerVariant(Pointer.Box(obj != null ? Pointer.Unbox(obj) : null, t), t);
                        }
                    }
                    else
                        return new ObjectVariant(v != null ? v.Value() : obj);
            }
        }

        public static TypeCode CalcTypeCode(BaseVariant v1, BaseVariant v2)
        {
            var type1 = v1.CalcTypeCode();
            var type2 = v2.CalcTypeCode();
            if (type1 == TypeCode.Empty || type1 == TypeCode.Object || type2 == TypeCode.Empty || type2 == TypeCode.Object)
                return TypeCode.Empty;

            // UInt32/UInt64 used for pointers
            if (type1 == TypeCode.UInt32)
                return (type2 == TypeCode.Int32) ? type1 : TypeCode.Empty;
            if (type2 == TypeCode.UInt32)
                return (type1 == TypeCode.Int32) ? type2 : TypeCode.Empty;
            if (type1 == TypeCode.UInt64)
                return (type2 == TypeCode.Int32 || type2 == TypeCode.Int64) ? type1 : TypeCode.Empty;
            if (type2 == TypeCode.UInt64)
                return (type1 == TypeCode.Int32 || type1 == TypeCode.Int64) ? type1 : TypeCode.Empty;

            if (type1 == TypeCode.Double || type2 == TypeCode.Double)
                return TypeCode.Double;
            if (type1 == TypeCode.Single || type2 == TypeCode.Single)
                return TypeCode.Single;
            if (type1 == TypeCode.Int64 || type2 == TypeCode.Int64)
                return TypeCode.Int64;
            return TypeCode.Int32;
        }
    }
}
