using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace VProtect.Runtime
{
    internal static unsafe class Utils
    {
        public static object VRead(ref byte* arrPtr)
        {
            var index = *(sbyte*)arrPtr;
            arrPtr += sizeof(sbyte);

            switch (index)
            {
                case 0:
                    {
                        var ret = *(bool*)arrPtr;
                        arrPtr += sizeof(bool);
                        return ret;
                    }
                case 1:
                    {
                        var ret = *(char*)arrPtr;
                        arrPtr += sizeof(char);
                        return ret;
                    }
                case 2:
                    {
                        var ret = *(byte*)arrPtr;
                        arrPtr += sizeof(byte);
                        return ret;
                    }
                case 3:
                    {
                        var ret = *(sbyte*)arrPtr;
                        arrPtr += sizeof(sbyte);
                        return ret;
                    }
                case 4:
                    {
                        var ret = *(short*)arrPtr;
                        arrPtr += sizeof(short);
                        return ret;
                    }
                case 5:
                    {
                        var ret = *(ushort*)arrPtr;
                        arrPtr += sizeof(ushort);
                        return ret;
                    }
                case 6:
                    {
                        var ret = *(int*)arrPtr;
                        arrPtr += sizeof(int);

                        return ret;
                    }
                case 7:
                    {
                        var ret = *(uint*)arrPtr;
                        arrPtr += sizeof(uint);
                        return ret;
                    }
                case 8:
                    {
                        var ret = *(long*)arrPtr;
                        arrPtr += sizeof(long);
                        return ret;
                    }
                case 9:
                    {
                        var ret = *(ulong*)arrPtr;
                        arrPtr += sizeof(ulong);
                        return ret;
                    }
                case 10:
                    {
                        var ret = *(float*)arrPtr;
                        arrPtr += sizeof(float);
                        return ret;
                    }
                case 11:
                    {
                        var ret = *(double*)arrPtr;
                        arrPtr += sizeof(double);
                        return ret;
                    }
                case 12:
                    {
                        var ret = *(decimal*)arrPtr;
                        arrPtr += sizeof(decimal);
                        return ret;
                    }
                case 13:
                    {
                        var len = ReadCompressedUInt(ref arrPtr);

                        var strBytes = new byte[len];
                        for (int i = 0; i < len; i++)
                            strBytes[i] = *(arrPtr + i);

                        arrPtr += len;

                        return Encoding.Unicode.GetString(strBytes);

                    }

                default:
                    return null;
            }
        }

        public static uint ReadCompressedUInt(ref byte* ptr)
        {
            uint num = 0;
            int shift = 0;
            do
            {
                num |= (*ptr & 0x7fu) << shift;
                shift += 7;
            } while ((*ptr++ & 0x80) != 0);
            return num;
        }

        public static uint FromCodedToken(uint codedToken)
        {
            uint rid = codedToken >> 3;
            switch ((int)(codedToken & 7))
            {
                case 1:
                    return rid | 0x02000000u;
                case 2:
                    return rid | 0x01000000u;
                case 3:
                    return rid | 0x1b000000u;
                case 4:
                    return rid | 0x0a000000u;
                case 5:
                    return rid | 0x06000000u;
                case 6:
                    return rid | 0x04000000u;
                case 7:
                    return rid | 0x2b000000u;
                default:
                    return rid;
            }
        }

        public static bool IsInst(this object obj, Type t)
        {
            if (obj == null)
                return true;

            var t2 = obj.GetType();
            if (t2 == t || t.IsAssignableFrom(t2))
                return true;

            return false;
        }

        public static void PushToFirst<T>(this List<T> list, T item)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "List is null.");
            }

            list.Insert(0, item);
        }

        public static void PushToEnd<T>(this List<T> list, T item)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list), "List is null.");

            list.Add(item);

        }

        public static T PopFromFirst<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("List is empty.");

            T firstElement = list[0];
            list.RemoveAt(0);

            return firstElement;
        }

        public static T PopFromEnd<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("List is empty.");

            T lastElement = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);

            return lastElement;
        }

        public static T Peek<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("List is empty.");

            return list[list.Count - 1];
        }
    }
}
