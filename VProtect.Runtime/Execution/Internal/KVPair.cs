using System;
using System.Text;

namespace VProtect.Runtime.Execution.Internal
{
    internal class KVPair<TKey, TValue>
    {
        public TKey Key
        {
            get;
            set;
        }

        public TValue Value
        {
            get;
            set;
        }

        public KVPair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append('[');

            if (Key != null)
                stringBuilder.Append(Key.ToString());

            stringBuilder.Append(", ");

            if (Value != null)
                stringBuilder.Append(Value.ToString());

            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }
    }
}
