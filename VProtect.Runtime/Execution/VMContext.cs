using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

using VProtect.Runtime.Data;
using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Variants;
using VProtect.Runtime.Execution.Internal;

namespace VProtect.Runtime.Execution
{
    internal class VMContext
    {
        private static readonly Dictionary<uint, object> _cache = new Dictionary<uint, object>();
        private static readonly Dictionary<MethodBase, DynamicMethod> _dynamicMethodCache = new Dictionary<MethodBase, DynamicMethod>();
        private readonly List<object> _stack = new List<object>();
        internal readonly List<TryBlock> _tryStack = new List<TryBlock>();
        internal readonly Stack<int> _finallyStack = new Stack<int>();
        internal Exception _exception;
        internal CatchBlock _filterBlock;

        public Module Module
        {
            get;
            private set;
        }

        public Module RTModule
        {
            get;
            private set;
        }

        public Module CorlibModule
        {
            get;
            private set;
        }

        public VMInstance Instance
        {
            get;
            private set;
        }

        public int ID
        {
            get;
            private set;
        }

        public object[] Args
        {
            get;
            private set;
        }

        public VMethodInfo VMethodInfo
        {
            get;
            private set;
        }

        public List<TryBlock> TryBlocks
        {
            get;
            private set;
        }

        public VInstruction[] Instructions
        {
            get;
            private set;
        }

        public List<KVPair<int, Type>> Constraineds
        {
            get;
            private set;
        }

        public int StackLength
        {
            get { return _stack.Count; }
        }

        public VMContext(Module module, VMInstance instance)
        {
            Module = module;
            Instance = instance;
            RTModule = typeof(VMContext).Module;
            CorlibModule = typeof(Module).Module;
        }
		
        public void SetSettings(int id, object[] args)
        {
            ID = id;
            Args = args;
            Constraineds = new List<KVPair<int, Type>>();

            VMethodInfo = Instance.Data.VMethodInfos[id];
            TryBlocks = Instance.Data.TryBlocks[id];
            Instructions = Instance.Data.VInstructions[id];
        }

        public void Push(BaseVariant value)
        {
            _stack.Add(value.ToStack());
        }

        public void PushValue(object value)
        {
            _stack.Add(value);
        }

        public BaseVariant Pop()
        {
            return (BaseVariant)_stack.PopFromEnd();
        }

        public BaseVariant Peek()
        {
            return (BaseVariant)_stack.Peek();
        }

        public object Return()
        {
            return _stack.PopFromEnd();
        }

        public void Clear()
        {
            _stack.Clear();
        }

        public bool Brtrue(BaseVariant v)
        {
            switch (v.CalcTypeCode())
            {
                case TypeCode.Object:
                    return v.Value() != null;
                default:
                    return v.ToInt32() != 0;
            }
        }

        public bool Bge(BaseVariant v1, BaseVariant v2, bool un)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();

                            return value2 >= value1;
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();

                            return value2 >= value1;
                        }
                    }
                case TypeCode.Int64:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();

                            return value2 >= value1;
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();

                            return value2 >= value1;
                        }
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();

                        return value2 >= value1;
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();

                        return value2 >= value1;
                    }
                case TypeCode.UInt32:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();

                            return value2 >= value1;
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();

                            return value2 >= value1;
                        }
                    }
                case TypeCode.UInt64:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();

                            return value2 >= value1;
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();

                            return value2 >= value1;
                        }
                    }
                default:
                    var value1_ = v1.Value();
                    var value2_ = v2.Value();
   
                    bool result = false;
                    if ((value2_ == value1_) || ((value2_ is IComparable) && (((IComparable)value2_).CompareTo(value1_) >= 0)))
                        result = true;

                    return result;
            }
        }

        public bool Ble(BaseVariant v1, BaseVariant v2, bool un)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();

                            return value2 <= value1;
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();

                            return value2 <= value1;
                        }
                    }
                case TypeCode.Int64:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();

                            return value2 <= value1;
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();

                            return value2 <= value1;
                        }
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();

                        return value2 <= value1;
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();

                        return value2 <= value1;
                    }
                case TypeCode.UInt32:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();

                            return value2 <= value1;
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();

                            return value2 <= value1;
                        }
                    }
                case TypeCode.UInt64:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();

                            return value2 <= value1;
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();

                            return value2 <= value1;
                        }
                    }
                default:
                    {
                        var value1 = v1.Value();
                        var value2 = v2.Value();

                        bool result = false;
                        if (value2 == value1 || (value2 is IComparable && ((IComparable)value2).CompareTo(value1) <= 0))
                            result = true;

                        return result;
                    }
            }
        }

        public bool Bne(BaseVariant v1, BaseVariant v2, bool un)
        {
            var type1 = v1.CalcTypeCode();
            switch (type1)
            {
                case TypeCode.Int32:
                    if (un)
                    {
                        var value1 = v1.ToUInt32();
                        var value2 = v2.ToUInt32();

                        return value2 != value1;
                    }
                    else
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt32();

                        return value2 != value1;
                    }
                case TypeCode.Int64:
                    if (un)
                    {
                        var value1 = v1.ToUInt64();
                        var value2 = v2.ToUInt64();

                        return value2 != value1;
                    }
                    else
                    {
                        var value1 = v1.ToInt64();
                        var value2 = v2.ToInt64();

                        return value2 != value1;
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();

                        return value2 != value1;
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();

                        return value2 != value1;
                    }
                default:
                    {
                        var value1 = v1.Value();
                        var value2 = v2.Value();

                        return value2 != value1;
                    }
            }
        }

        public BaseVariant CeqAndBeq(BaseVariant v1, BaseVariant v2)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt32();

                        return new BoolVariant(value2 == value1);
                    }
                case TypeCode.Int64:
                    {
                        var value1 = v1.ToInt64();
                        var value2 = v2.ToInt64();

                        return new BoolVariant(value2 == value1);
                    }
                case TypeCode.Single:
                    {
                        var value1 = v1.ToSingle();
                        var value2 = v2.ToSingle();

                        return new BoolVariant(value2 == value1);
                    }
                case TypeCode.Double:
                    {
                        var value1 = v1.ToDouble();
                        var value2 = v2.ToDouble();

                        return new BoolVariant(value2 == value1);
                    }
                case TypeCode.UInt32:
                    {
                        var value1 = v1.ToUInt32();
                        var value2 = v2.ToUInt32();

                        return new BoolVariant(value2 == value1);
                    }
                case TypeCode.UInt64:
                    {
                        var value1 = v1.ToUInt64();
                        var value2 = v2.ToUInt64();

                        return new BoolVariant(value2 == value1);
                    }
                default:
                    var value1_ = v1.Value();
                    var value2_ = v2.Value();

                    return new BoolVariant(value2_ == value1_);
            }
        }

        public BaseVariant CgtAndBgt(BaseVariant v1, BaseVariant v2, bool un)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();

                            return new BoolVariant(value2 > value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();

                            return new BoolVariant(value2 > value1);
                        }
                    }
                case TypeCode.Int64:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();

                            return new BoolVariant(value2 > value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();

                            return new BoolVariant(value2 > value1);
                        }
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();

                        return new BoolVariant(value2 > value1);
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();

                        return new BoolVariant(value2 > value1);
                    }
                case TypeCode.UInt32:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();

                            return new BoolVariant(value2 > value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();

                            return new BoolVariant(value2 > value1);
                        }
                    }
                case TypeCode.UInt64:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();

                            return new BoolVariant(value2 > value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();

                            return new BoolVariant(value2 > value1);
                        }
                    }
                default:
                    var value1_ = v1.Value();
                    var value2_ = v2.Value();

                    return new BoolVariant(Comparer<object>.Default.Compare(value2_, value1_) > 0);
            }
        }

        public BaseVariant CltAndBlt(BaseVariant v1, BaseVariant v2, bool un)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();

                            return new BoolVariant(value2 < value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();

                            return new BoolVariant(value2 < value1);
                        }
                    }
                case TypeCode.Int64:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();

                            return new BoolVariant(value2 < value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();

                            return new BoolVariant(value2 < value1);
                        }
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();

                        return new BoolVariant(value2 < value1);
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();

                        return new BoolVariant(value2 < value1);
                    }
                case TypeCode.UInt32:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();

                            return new BoolVariant(value2 < value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();

                            return new BoolVariant(value2 < value1);
                        }
                    }
                case TypeCode.UInt64:
                    {
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();

                            return new BoolVariant(value2 < value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();

                            return new BoolVariant(value2 < value1);
                        }
                    }
                default:
                    var value1_ = v1.Value();
                    var value2_ = v2.Value();

                    return new BoolVariant(((IComparable)value2_).CompareTo(value1_) < 0);
            }
        }

        public BaseVariant Add(BaseVariant v1, BaseVariant v2, bool ovf, bool un)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        int value;
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();
                            value = ovf ? (int)checked(value2 + value1) : (int)(value2 + value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();
                            value = ovf ? checked(value2 + value1) : (value2 + value1);
                        }

                        return new IntVariant(value);
                    }
                case TypeCode.Int64:
                    {
                        long value;
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();
                            value = ovf ? (long)checked(value2 + value1) : (long)(value2 + value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();
                            value = ovf ? checked(value2 + value1) : (value2 + value1);
                        }

                        return new LongVariant(value);
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();
                        return new SingleVariant(ovf ? checked(value2 + value1) : (value2 + value1));
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();
                        return new DoubleVariant(ovf ? checked(value2 + value1) : (value2 + value1));
                    }
                case TypeCode.UInt32:
                    {
                        int value;
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();
                            value = ovf ? (int)checked(value2 + value1) : (int)(value2 + value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();
                            value = ovf ? checked(value2 + value1) : (value2 + value1);
                        }

                        PointerVariant v = v1.CalcTypeCode() == type ? (PointerVariant)v2 : (PointerVariant)v1;
                        unsafe
                        {
                            return new PointerVariant(Pointer.Box(new IntPtr(value).ToPointer(), v.Type()), v.Type());
                        }
                    }
                case TypeCode.UInt64:
                    {
                        long value;
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();
                            value = ovf ? (long)checked(value2 + value1) : (long)(value2 + value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();
                            value = ovf ? checked(value2 + value1) : (value2 + value1);
                        }

                        PointerVariant v = v1.CalcTypeCode() == type ? (PointerVariant)v2 : (PointerVariant)v1;
                        unsafe
                        {
                            return new PointerVariant(Pointer.Box(new IntPtr(value).ToPointer(), v.Type()), v.Type());
                        }
                    }
                default:
                    {
                        throw new OverflowException();
                    }
            }
        }

        public BaseVariant Sub(BaseVariant v1, BaseVariant v2, bool ovf, bool un)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        int value;
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();
                            value = ovf ? (int)checked(value2 - value1) : (int)(value2 - value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();
                            value = ovf ? checked(value2 - value1) : (value2 - value1);
                        }
                        return new IntVariant(value);
                    }
                case TypeCode.Int64:
                    {
                        long value;
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();
                            value = ovf ? (long)checked(value2 - value1) : (long)(value2 - value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();
                            value = ovf ? checked(value2 - value1) : (value2 - value1);
                        }
                        return new LongVariant(value);
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();
                        return new SingleVariant(ovf ? checked(value2 - value1) : (value2 - value1));
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();
                        return new DoubleVariant(ovf ? checked(value2 - value1) : (value2 - value1));
                    }
                case TypeCode.UInt32:
                    {
                        int value;
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();
                            value = ovf ? (int)checked(value2 - value1) : (int)(value2 - value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();
                            value = ovf ? checked(value2 - value1) : (value2 - value1);
                        }

                        PointerVariant v = v1.CalcTypeCode() == type ? (PointerVariant)v2 : (PointerVariant)v1;
                        unsafe
                        {
                            return new PointerVariant(Pointer.Box(new IntPtr(value).ToPointer(), v.Type()), v.Type());
                        }
                    }
                case TypeCode.UInt64:
                    {
                        long value;
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();
                            value = ovf ? (long)checked(value2 - value1) : (long)(value2 - value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();
                            value = ovf ? checked(value2 - value1) : (value2 - value1);
                        }

                        PointerVariant v = v1.CalcTypeCode() == type ? (PointerVariant)v2 : (PointerVariant)v1;
                        unsafe
                        {
                            return new PointerVariant(Pointer.Box(new IntPtr(value).ToPointer(), v.Type()), v.Type());
                        }
                    }
                default:
                    {
                        throw new OverflowException();
                    }
            }
        }

        public BaseVariant Mul(BaseVariant v1, BaseVariant v2, bool ovf, bool un)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        int value;
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();

                            value = ovf ? (int)checked(value2 * value1) : (int)(value2 * value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();

                            value = ovf ? checked(value2 * value1) : (value2 * value1);
                        }
                        return new IntVariant(value);
                    }
                case TypeCode.Int64:
                    {
                        long value;
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();

                            value = ovf ? (long)checked(value2 * value1) : (long)(value2 * value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();

                            value = ovf ? checked(value2 * value1) : (value2 * value1);
                        }
                        return new LongVariant(value);
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();

                        return new DoubleVariant(ovf ? checked(value2 * value1) : (value2 * value1));
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();

                        return new DoubleVariant(ovf ? checked(value2 * value1) : (value2 * value1));
                    }
                default:
                    {
                        throw new OverflowException();
                    }
            }
        }

        public BaseVariant Div(BaseVariant v1, BaseVariant v2, bool un)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        int value;
                        if (un)
                        {
                            var value1 = v1.ToUInt32();
                            var value2 = v2.ToUInt32();
                            value = (int)(value2 / value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt32();
                            var value2 = v2.ToInt32();
                            value = value2 / value1;
                        }

                        return new IntVariant(value);
                    }
                case TypeCode.Int64:
                    {
                        long value;
                        if (un)
                        {
                            var value1 = v1.ToUInt64();
                            var value2 = v2.ToUInt64();
                            value = (long)(value2 / value1);
                        }
                        else
                        {
                            var value1 = v1.ToInt64();
                            var value2 = v2.ToInt64();
                            value = value2 / value1;
                        }

                        return new LongVariant(value);
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();

                        return new SingleVariant(value2 / value1);
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();

                        return new DoubleVariant(value2 / value1);
                    }
                default:
                    {
                        throw new ArithmeticException();
                    }
            }
        }

        public BaseVariant Rem(BaseVariant v1, BaseVariant v2, bool un)
        {
            var type1 = v1.CalcTypeCode();
            switch (type1)
            {
                case TypeCode.Int32:
                    if (un)
                    {
                        var value1 = v1.ToUInt32();
                        var value2 = v2.ToUInt32();

                        return new IntVariant((int)(value2 % value1));
                    }
                    else
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt32();

                        return new IntVariant(value2 % value1);
                    }
                case TypeCode.Int64:
                    if (un)
                    {
                        var value1 = v1.ToUInt64();
                        var value2 = v2.ToUInt64();

                        return new LongVariant((long)(value2 % value1));
                    }
                    else
                    {
                        var value1 = v1.ToInt64();
                        var value2 = v2.ToInt64();

                        return new LongVariant(value2 % value1);
                    }
                case TypeCode.Single:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToSingle();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToSingle();

                        return new DoubleVariant(value2 % value1);
                    }
                case TypeCode.Double:
                    {
                        var value1 = (un ? v1.ToUnsigned() : v1).ToDouble();
                        var value2 = (un ? v2.ToUnsigned() : v2).ToDouble();

                        return new DoubleVariant(value2 % value1);
                    }
                default:
                    {
                        throw new DivideByZeroException();
                    }
            }
        }

        public BaseVariant Xor(BaseVariant v1, BaseVariant v2)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt32();

                        return new IntVariant(value2 ^ value1);
                    }
                case TypeCode.Int64:
                    {
                        var value1 = v1.ToInt64();
                        var value2 = v2.ToInt64();

                        return new LongVariant(value2 ^ value1);
                    }
                case TypeCode.Single:
                    return new SingleVariant((IntPtr.Size == 4) ? float.NaN : (float)0);
                case TypeCode.Double:
                    return new DoubleVariant((IntPtr.Size == 4) ? double.NaN : (double)0);
                case TypeCode.UInt32:
                    {
                        var value1 = v1.ToUInt32();
                        var value2 = v2.ToUInt32();

                        return new UIntVariant(value2 ^ value1);
                    }
                case TypeCode.UInt64:
                    {
                        var value1 = v1.ToUInt64();
                        var value2 = v2.ToUInt64();

                        return new ULongVariant(value2 ^ value1);
                    }
                default:
                    throw new OverflowException();
            }
        }

        public BaseVariant Or(BaseVariant v1, BaseVariant v2)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt32();

                        return new IntVariant(value2 | value1);
                    }
                case TypeCode.Int64:
                    {
                        var value1 = v1.ToInt64();
                        var value2 = v2.ToInt64();

                        return new LongVariant(value2 | value1);
                    }
                case TypeCode.Single:
                    return new SingleVariant((IntPtr.Size == 4) ? float.NaN : (float)0);
                case TypeCode.Double:
                    return new DoubleVariant((IntPtr.Size == 4) ? double.NaN : (double)0);
                case TypeCode.UInt32:
                    {
                        var value1 = v1.ToUInt32();
                        var value2 = v2.ToUInt32();

                        return new UIntVariant(value2 | value1);
                    }
                case TypeCode.UInt64:
                    {
                        var value1 = v1.ToUInt64();
                        var value2 = v2.ToUInt64();

                        return new ULongVariant(value2 | value1);
                    }
                default:
                    {
                        throw new OverflowException();
                    }
            }
        }

        public BaseVariant And(BaseVariant v1, BaseVariant v2)
        {
            var type = BaseVariant.CalcTypeCode(v1, v2);
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt32();

                        return new IntVariant(value2 & value1);
                    }
                case TypeCode.Int64:
                    {
                        var value1 = v1.ToInt64();
                        var value2 = v2.ToInt64();

                        return new LongVariant(value2 & value1);
                    }
                case TypeCode.Single:
                    return new SingleVariant((IntPtr.Size == 4) ? float.NaN : (float)0);
                case TypeCode.Double:
                    return new DoubleVariant((IntPtr.Size == 4) ? double.NaN : (double)0);
                case TypeCode.UInt32:
                    {
                        var value1 = v1.ToUInt32();
                        var value2 = v2.ToUInt32();

                        return new UIntVariant(value2 & value1);
                    }
                case TypeCode.UInt64:
                    {
                        var value1 = v1.ToUInt64();
                        var value2 = v2.ToUInt64();

                        return new ULongVariant(value2 & value1);
                    }
                default:
                    {
                        throw new OverflowException();
                    }
            }
        }

        public BaseVariant Shl(BaseVariant v1, BaseVariant v2)
        {
            var type = v1.CalcTypeCode();
            switch (type)
            {
                case TypeCode.Int32:
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt32();

                        return new IntVariant(value2 << value1);
                    }
                case TypeCode.Int64:
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt64();

                        return new LongVariant(value2 << value1);
                    }
                default:
                    throw new OverflowException();
            }
        }

        public BaseVariant Shr(BaseVariant v1, BaseVariant v2, bool un)
        {
            var type = v1.CalcTypeCode();
            switch (type)
            {
                case TypeCode.Int32:
                    if (un)
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToUInt32();

                        return new IntVariant((int)(value2 >> value1));
                    }
                    else
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt32();

                        return new IntVariant(value2 >> value1);
                    }
                case TypeCode.Int64:
                    if (un)
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToUInt64();

                        return new LongVariant((long)(value2 >> value1));
                    }
                    else
                    {
                        var value1 = v1.ToInt32();
                        var value2 = v2.ToInt64();

                        return new LongVariant(value2 >> value1);
                    }
                default:
                    {
                        throw new OverflowException();
                    }
            }
        }

        public BaseVariant NegAndNot(BaseVariant v, bool neg)
        {
            switch (v.CalcTypeCode())
            {
                case TypeCode.Int16:
                    return neg ? new IntVariant(-v.ToInt32()) : new IntVariant(~v.ToInt32());
                case TypeCode.Int32:
                    return neg ? new IntVariant(-v.ToInt32()) : new IntVariant(~v.ToInt32());
                case TypeCode.Int64:
                    return neg ? new LongVariant(-v.ToInt64()) : new LongVariant(~v.ToInt64());
                case TypeCode.Single:
                    if (neg)
                        return new SingleVariant(-v.ToSingle());
                    else
                        throw new InvalidOperationException();
                case TypeCode.Double:
                    if (neg)
                        return new DoubleVariant(-v.ToSingle());
                    else
                        throw new InvalidOperationException();
                case TypeCode.UInt16:
                    return neg ? new IntVariant(-v.ToInt32()) : new IntVariant(~v.ToInt32());
                default:
                    throw new InvalidOperationException();
            }
        }

        public BaseVariant Newobj(MethodBase method)
        {
            var refs = new Dictionary<int, BaseVariant>();

            var parameters = method.GetParameters();
            var paramTypes = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                paramTypes[i] = parameters[i].ParameterType;

            var args = new object[parameters.Length];
            var gcConverter = new UnmanagedObjectHandler();
            try
            {
                int j = args.Length - 1;
                while (j >= 0)
                {
                    Type type = paramTypes[j];
                    BaseVariant argVar = Pop();

                    if (argVar.IsReference())
                    {
                        if (type.IsByRef)
                            refs[j] = argVar;
                        else
                            args[j] = gcConverter.PinObject(argVar.Value());
                    }
                    else
                        args[j] = argVar.Convert(type).Value();

                    j--;
                }

                var res = ((ConstructorInfo)method).Invoke(args);
                foreach (var r in refs)
                    r.Value.SetValue(args[r.Key]);

                return BaseVariant.Convert(res, method.DeclaringType);
            }
            finally
            {
                gcConverter.Dispose();
            }
        }

        public BaseVariant Call(MethodBase method, Type methodType, bool virt)
        {
            var info = method as MethodInfo;

            Type[] paramTypes = null;

            if (method.CallingConvention == CallingConventions.VarArgs)
            {
                // maybe later...
                Console.WriteLine("user76");
            }
            else
            {
                var parameters = method.GetParameters();

                paramTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    paramTypes[i] = parameters[i].ParameterType;
            }

            BindingFlags invokeFlags;
#if NETCOREAPP
        			invokeFlags = BindingFlags.DoNotWrapExceptions;
#else
            invokeFlags = BindingFlags.Default;
#endif

            var refs = new Dictionary<int, BaseVariant>();
            var args = new object[paramTypes.Length];
            var gcHandler = new UnmanagedObjectHandler();

            try
            {
                int j = args.Length - 1;

                while (j >= 0)
                {
                    Type type = paramTypes[j];
                    BaseVariant argVar = Pop();
                   
                    if (!argVar.IsReference())
                        goto IL_DD2;

                    if (type.IsByRef)
                    {
                        refs[j] = argVar;
                        goto IL_DD2;
                    }

                    args[j] = gcHandler.PinObject(argVar.Value());
             
                IL_DD2:
                    args[j] = argVar.Convert(type).Value();
                    goto IL_E10;
                IL_E10:
                    j--;
                    continue;
                }

                BaseVariant val = method.IsStatic ? null : Pop();
                object obj = val?.Value() ?? null;
                if (virt && obj == null)
                    throw new NullReferenceException();

                object res = null;
                if (method.CallingConvention == CallingConventions.VarArgs)
                {
                    // maybe later...
                    Console.WriteLine("user76");
                }
                else if (!virt && method.IsVirtual && !method.IsFinal)
                {
                    var paramValues = new object[paramTypes.Length + 1];
                    paramValues[0] = obj;
                    for (var i = 0; i < paramTypes.Length; i++)
                        paramValues[i + 1] = args[i];

                    DynamicMethod dynamicMethod;
                    lock (_dynamicMethodCache)
                    {
                        if (!_dynamicMethodCache.TryGetValue(method, out dynamicMethod))
                        {
                            var paramTypes_ = new Type[paramValues.Length];
                            paramTypes_[0] = method.DeclaringType;

                            for (int m = 0; m < paramTypes.Length; m++)
                                paramTypes_[m + 1] = paramTypes[m];

                            dynamicMethod = new DynamicMethod(method.Name, info != null && info.ReturnType != typeof(void) ? info.ReturnType : null, paramTypes_, Unverifier.Module, true);
                            var gen = dynamicMethod.GetILGenerator();

                            for (int n = 0; n < paramTypes_.Length; n++)
                            {
                                if (n == 0 && method.DeclaringType.IsValueType)
                                    gen.Emit(System.Reflection.Emit.OpCodes.Ldarga, n);
                                else
                                    gen.Emit(System.Reflection.Emit.OpCodes.Ldarg, n);
                            }

                            gen.Emit(System.Reflection.Emit.OpCodes.Call, info);
                            gen.Emit(System.Reflection.Emit.OpCodes.Ret);

                            _dynamicMethodCache[method] = dynamicMethod;
                        }
                    }

                    res = dynamicMethod.Invoke(null, invokeFlags, null, paramValues, null);

                    foreach (var r in refs)
                        r.Value.SetValue(paramValues[r.Key + 1]);

                    refs.Clear();
                }
                else
                {
                    if (method.IsConstructor && method.DeclaringType.IsValueType && Nullable.GetUnderlyingType(method.DeclaringType) != null)
                        obj = ((ConstructorInfo)method).Invoke(invokeFlags, null, args, null);
                    else
                        res = method.Invoke(obj, invokeFlags, null, args, null);

                    if (val != null && val.IsReference() && method.DeclaringType.IsValueType)
                        val.SetValue(obj);
                }

                foreach (var r in refs)
                    r.Value.SetValue(args[r.Key]);

                return (info != null && info.ReturnType != typeof(void)) ? BaseVariant.Convert(res, info.ReturnType) : null;
            }
            finally
            {
                gcHandler.Dispose();
            }
        }

        public Type GetType(uint token, Module module = null)
        {
            lock (_cache)
            {
                object stored;
                if (_cache.TryGetValue(token, out stored))
                    return (Type)stored;
                else
                {
                    var codedToken = Utils.FromCodedToken(token);
                    var type = module == null ?  Module.ResolveType((int)codedToken) : module.ResolveType((int)codedToken);
                   
                    _cache.Add(token, type);

                    return type;
                }
            }
        }

        public MethodBase GetMethod(uint token, Module module = null)
        {
            lock (_cache)
            {
                object stored;
                if (_cache.TryGetValue(token, out stored))
                    return (MethodBase)stored;
                else
                {
                    var codedToken = Utils.FromCodedToken(token);
                    var method = module == null ? Module.ResolveMethod((int)codedToken) : module.ResolveMethod((int)codedToken);
                    
                    _cache.Add(token, method);

                    return method;
                }
            }
        }

        public FieldInfo GetField(uint token, Module module = null)
        {
            lock (_cache)
            {
                object stored;
                if (_cache.TryGetValue(token, out stored))
                    return (FieldInfo)stored;
                else
                {
                    var codedToken = Utils.FromCodedToken(token);
                    var field = module == null ? Module.ResolveField((int)codedToken) : module.ResolveField((int)codedToken);

                    _cache.Add(token, field);

                    return field;
                }
            }
        }

        public object RunInternal(object[] args)
        {
            Push(new ArrayVariant(args));

            int opcodeOffset = 0;
            do
            {
                try
                {
                    Entertry(opcodeOffset);

                    var instruction = Instructions[opcodeOffset];
                    if (instruction.ILCode == Constants.Constrained)
                    {
                        opcodeOffset++;
                        Constraineds.Add(new KVPair<int, Type>(opcodeOffset, GetType((uint)instruction.Operands[0])));
                    }
                    else if (instruction.ILCode == Constants.Br || instruction.ILCode == Constants.Nop)
                        opcodeOffset = instruction.Operands.Length > 0 ? (int)instruction.Operands[0] : (opcodeOffset + 1);
                    else
                    {
                        var opcode = OpCodeMap.Lookup(instruction.ILCode);
                        opcode.Run(this, ref opcodeOffset);
                    }
                }
                catch (Exception e)
                {
                    if (_filterBlock == null)
                        _exception = e;

                    Unwind(ref opcodeOffset);
                }
        } while (Instructions.Length > opcodeOffset);

            return Return();
        }

        private void Entertry(int opcodeOffset)
        {
            foreach (var current in TryBlocks)
            {
                if (current.Begin() == opcodeOffset)
                {
                    if (_tryStack.Count > 0)
                    {
                        var tryStck = _tryStack.FirstOrDefault(ts => ts == current);

                        if (tryStck == null)
                            _tryStack.Add(current);
                    }
                    else
                        _tryStack.Add(current);
                }
            }
        }

        public void Unwind(ref int opcodeOffset)
        {
            _stack.Clear();
            _finallyStack.Clear();

            while (_tryStack.Count != 0)
            {
                var catchBlocks = _tryStack.Peek().CatchBlocks();
                int startIndex = (_filterBlock == null) ? 0 : catchBlocks.IndexOf(_filterBlock) + 1;
                _filterBlock = null;

                for (var i = startIndex; i < catchBlocks.Count; i++)
                {
                    var current = catchBlocks[i];
                    switch (current.Type())
                    {
                        case 0:
                            var type = _exception.GetType();
                            var type2 = current.CatchType();
                            if (type == type2 || type.IsSubclassOf(type2))
                            {
                                _tryStack.PopFromEnd();
                                _stack.Add(new ObjectVariant(_exception));

                                opcodeOffset = current.Handler();
                                return;
                            }
                            break;
                        case 1:
                            _filterBlock = current;
                            _stack.Add(new ObjectVariant(_exception));

                            opcodeOffset = current.Filter();
                            return;
                    }
                }

                _tryStack.PopFromEnd();
                for (var i = catchBlocks.Count; i > 0; i--)
                {
                    var current = catchBlocks[i - 1];
                    if (current.Type() == 2 || current.Type() == 4)
                        _finallyStack.Push(current.Handler());
                }

                if (_finallyStack.Count != 0)
                {
                    opcodeOffset = _finallyStack.Pop();
                    return;
                }
            }

            throw _exception;
        }

        public void Release()
        {
            _tryStack.Clear();
            _finallyStack.Clear();

            _exception = null;
            _filterBlock = null;

            Module = null;
            RTModule = null;
            CorlibModule = null;

            ID = 0;
            Args = null;
            VMethodInfo = null;
            TryBlocks = null;
            Instructions = null;
            Constraineds.Clear();
            Instance = null;

            Clear();
        }
    }
}