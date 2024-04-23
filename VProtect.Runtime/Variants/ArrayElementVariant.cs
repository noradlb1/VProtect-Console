using System;
using System.Reflection;
using System.Reflection.Emit;

using VProtect.Runtime.Execution.Internal;

namespace VProtect.Runtime.Variants
{
    internal sealed class ArrayElementVariant : StackableVariant
    {
        private Array _value;
        private int _index;

        public ArrayElementVariant(Array value, int index)
        {
            _value = value;
            _index = index;
        }

        public override Type Type()
        {
            return _value.GetType().GetElementType();
        }

        public override BaseVariant Clone()
        {
            return new ArrayElementVariant(_value, _index);
        }

        public override bool IsReference()
        {
            return true;
        }

        public override object Value()
        {
            return _value.GetValue(_index);
        }

        public override void SetValue(object value)
        {
            switch (System.Type.GetTypeCode(_value.GetType().GetElementType()))
            {
                case TypeCode.SByte:
                    value = System.Convert.ToSByte(value);
                    break;
                case TypeCode.Byte:
                    value = System.Convert.ToByte(value);
                    break;
                case TypeCode.Char:
                    value = System.Convert.ToChar(value);
                    break;
                case TypeCode.Int16:
                    value = System.Convert.ToInt16(value);
                    break;
                case TypeCode.UInt16:
                    value = System.Convert.ToUInt16(value);
                    break;
                case TypeCode.Int32:
                    value = System.Convert.ToInt32(value);
                    break;
                case TypeCode.UInt32:
                    value = System.Convert.ToUInt32(value);
                    break;
                case TypeCode.Int64:
                    value = System.Convert.ToInt64(value);
                    break;
                case TypeCode.UInt64:
                    value = System.Convert.ToUInt64(value);
                    break;
            }

            _value.SetValue(value, _index);
        }

        public override void SetFieldValue(FieldInfo field, object value)
        {
            var obj = Value();
            field.SetValue(obj, value);

            if (obj is ValueType)
                SetValue(obj);
        }

        public override UIntPtr ToUIntPtr()
        {
            var dynamicMethod = new DynamicMethod("", typeof(UIntPtr), new Type[] { _value.GetType(), typeof(int) }, Unverifier.Module, true);
            var gen = dynamicMethod.GetILGenerator();
            gen.Emit(System.Reflection.Emit.OpCodes.Ldarg, 0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldarg, 1);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldelema, _value.GetType().GetElementType());
            gen.Emit(System.Reflection.Emit.OpCodes.Conv_U);
            gen.Emit(System.Reflection.Emit.OpCodes.Ret);

            return (UIntPtr)dynamicMethod.Invoke(null, new object[] { _value, _index });
        }
    }
}
