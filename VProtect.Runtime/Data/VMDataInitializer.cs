using System;
using System.Runtime.InteropServices;

namespace VProtect.Runtime.Data
{
    internal unsafe class VMDataInitializer
    {
        internal static void* GetData(VMData vmdata)
        {
            var module = typeof(VMDataInitializer).Module;
            var moduleBase = (byte*)Marshal.GetHINSTANCE(module);

            string fqn = module.FullyQualifiedName;
            bool isFlat = fqn.Length > 0 && fqn[0] == '<';
            if (isFlat)
                return GetDataStreamFlat(moduleBase);
            else
                return GetDataStreamMapped(moduleBase);
        }

        static void* GetDataStreamMapped(byte* moduleBase)
        {
            byte* ptr = moduleBase + 0x3c;

            byte* ptr2;
            ptr = ptr2 = moduleBase + *(uint*)ptr;
            ptr += 0x6;

            ushort sectNum = *(ushort*)ptr;
            ptr += 14;

            ushort optSize = *(ushort*)ptr;
            ptr = ptr2 = ptr + 0x4 + optSize;

            byte* mdDir = moduleBase + *(uint*)(ptr - 16);

            byte* mdHdr = moduleBase + *(uint*)(mdDir + 8);
            mdHdr += 12;
            mdHdr += *(uint*)mdHdr;
            mdHdr = (byte*)(((ulong)mdHdr + 7) & ~3UL);
            mdHdr += 4;

            uint offset = *(uint*)mdHdr;
            uint len = *(uint*)(mdHdr + 4);
            mdHdr += 8;

            void* data = (void*)Marshal.AllocHGlobal((int)len);
            IntPtr sourcePtr = (IntPtr)(moduleBase + *(uint*)(mdDir + 8) + offset);

            byte[] buffer = new byte[len];
            Marshal.Copy(sourcePtr, buffer, 0, (int)len);
            Marshal.Copy(buffer, 0, (IntPtr)data, (int)len);

            return data;
        }

        static void* GetDataStreamFlat(byte* moduleBase)
        {
            byte* ptr = moduleBase + 0x3c;

            byte* ptr2;
            ptr = ptr2 = moduleBase + *(uint*)ptr;
            ptr += 0x6;

            ushort sectNum = *(ushort*)ptr;
            ptr += 14;

            ushort optSize = *(ushort*)ptr;
            ptr = ptr2 = ptr + 0x4 + optSize;

            uint mdDir = *(uint*)(ptr - 16);

            var vAdrs = new uint[sectNum];
            var vSizes = new uint[sectNum];
            var rAdrs = new uint[sectNum];
            for (int i = 0; i < sectNum; i++)
            {
                vAdrs[i] = *(uint*)(ptr + 12);
                vSizes[i] = *(uint*)(ptr + 8);
                rAdrs[i] = *(uint*)(ptr + 20);
                ptr += 0x28;
            }

            for (int i = 0; i < sectNum; i++)
                if (vAdrs[i] <= mdDir && mdDir < vAdrs[i] + vSizes[i])
                {
                    mdDir = mdDir - vAdrs[i] + rAdrs[i];
                    break;
                }

            byte* mdDirPtr = moduleBase + mdDir;
            uint mdHdr = *(uint*)(mdDirPtr + 8);
            for (int i = 0; i < sectNum; i++)
                if (vAdrs[i] <= mdHdr && mdHdr < vAdrs[i] + vSizes[i])
                {
                    mdHdr = mdHdr - vAdrs[i] + rAdrs[i];
                    break;
                }


            byte* mdHdrPtr = moduleBase + mdHdr;
            mdHdrPtr += 12;
            mdHdrPtr += *(uint*)mdHdrPtr;
            mdHdrPtr = (byte*)(((ulong)mdHdrPtr + 7) & ~3UL);
            mdHdrPtr += 4;

            uint offset = *(uint*)mdHdrPtr;
            uint len = *(uint*)(mdHdrPtr + 4);
            mdHdrPtr += 8;

            void* data = (void*)Marshal.AllocHGlobal((int)len);
            IntPtr sourcePtr = (IntPtr)(moduleBase + mdHdr + offset);

            byte[] buffer = new byte[len];
            Marshal.Copy(sourcePtr, buffer, 0, (int)len);
            Marshal.Copy(buffer, 0, (IntPtr)data, (int)len);

            return data;
        }
    }
}
