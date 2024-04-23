using System;
using System.Reflection;
using System.Runtime.InteropServices;

#pragma warning disable CS0675
namespace VProtect.Core.VM.InjRuntime
{
    internal static unsafe class Merge
    {
        static void cctor()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var module = typeof(Merge).Module;

                byte[] data = null;
                if (module.FullyQualifiedName.Length > 0 && module.FullyQualifiedName[0] == '<')
                    data = GetFirstHeapStreamFlat(module);
                else
                    data = GetFirstHeapStreamMapped(module);

                return Assembly.Load(data);
            }
            catch { return null; }
        }

        private static byte[] GetFirstHeapStreamMapped(Module module)
        {
            byte* moduleBase = (byte*)Marshal.GetHINSTANCE(module);
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
            var len = *(uint*)(mdHdr + 4);
            mdHdr += 8;

            var data = new byte[len];
            Marshal.Copy((IntPtr)(moduleBase + *(uint*)(mdDir + 8) + offset), data, 0, (int)len);

            return Decompress(data);
        }

        private static byte[] GetFirstHeapStreamFlat(Module module)
        {
            byte* moduleBase = (byte*)Marshal.GetHINSTANCE(module);
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

            var data = new byte[len];
            Marshal.Copy((IntPtr)(moduleBase + mdHdr + offset), data, 0, (int)len);

            return Decompress(data);
        }

        public static byte[] Decompress(byte[] source)
        {
            int size = 0;

            if ((((source[0] & 2) == 2) ? 9 : 3) == 9)
                size = source[5] | (source[6] << 8) | (source[7] << 16) | (source[8] << 24);
            else
                size = source[2];

            int src = ((source[0] & 2) == 2) ? 9 : 3;
            int dst = 0;
            uint cword_val = 1;
            byte[] destination = new byte[size];
            int[] hashtable = new int[4096];
            byte[] hash_counter = new byte[4096];
            int last_matchstart = size - 10 - 1;
            int last_hashed = -1;
            int hash;
            uint fetch = 0;

            int level = (source[0] >> 2) & 0x3;

            if (level != 1 && level != 3)
                return null;

            if ((source[0] & 1) != 1)
            {
                byte[] d2 = new byte[size];
                Array.Copy(source, ((source[0] & 2) == 2) ? 9 : 3, d2, 0, size);

                return d2;
            }

            for (;;)
            {
                if (cword_val == 1)
                {
                    cword_val = (uint)(source[src] | (source[src + 1] << 8) | (source[src + 2] << 16) | (source[src + 3] << 24));
                    src += 4;
                    if (dst <= last_matchstart)
                    {
                        if (level == 1)
                            fetch = (uint)(source[src] | (source[src + 1] << 8) | (source[src + 2] << 16));
                        else
                            fetch = (uint)(source[src] | (source[src + 1] << 8) | (source[src + 2] << 16) | (source[src + 3] << 24));
                    }
                }

                if ((cword_val & 1) == 1)
                {
                    uint matchlen;
                    uint offset2;

                    cword_val = cword_val >> 1;

                    if (level == 1)
                    {
                        hash = ((int)fetch >> 4) & 0xfff;
                        offset2 = (uint)hashtable[hash];

                        if ((fetch & 0xf) != 0)
                        {
                            matchlen = (fetch & 0xf) + 2;
                            src += 2;
                        }
                        else
                        {
                            matchlen = source[src + 2];
                            src += 3;
                        }
                    }
                    else
                    {
                        uint offset;
                        if ((fetch & 3) == 0)
                        {
                            offset = (fetch & 0xff) >> 2;
                            matchlen = 3;
                            src++;
                        }
                        else if ((fetch & 2) == 0)
                        {
                            offset = (fetch & 0xffff) >> 2;
                            matchlen = 3;
                            src += 2;
                        }
                        else if ((fetch & 1) == 0)
                        {
                            offset = (fetch & 0xffff) >> 6;
                            matchlen = ((fetch >> 2) & 15) + 3;
                            src += 2;
                        }
                        else if ((fetch & 127) != 3)
                        {
                            offset = (fetch >> 7) & 0x1ffff;
                            matchlen = ((fetch >> 2) & 0x1f) + 2;
                            src += 3;
                        }
                        else
                        {
                            offset = (fetch >> 15);
                            matchlen = ((fetch >> 7) & 255) + 3;
                            src += 4;
                        }
                        offset2 = (uint)(dst - offset);
                    }

                    destination[dst + 0] = destination[offset2 + 0];
                    destination[dst + 1] = destination[offset2 + 1];
                    destination[dst + 2] = destination[offset2 + 2];

                    for (int i = 3; i < matchlen; i += 1)
                    {
                        destination[dst + i] = destination[offset2 + i];
                    }

                    dst += (int)matchlen;

                    if (level == 1)
                    {
                        fetch = (uint)(destination[last_hashed + 1] | (destination[last_hashed + 2] << 8) | (destination[last_hashed + 3] << 16));
                        while (last_hashed < dst - matchlen)
                        {
                            last_hashed++;
                            hash = (int)(((fetch >> 12) ^ fetch) & 4095);
                            hashtable[hash] = last_hashed;
                            hash_counter[hash] = 1;
                            fetch = (uint)(fetch >> 8 & 0xffff | destination[last_hashed + 3] << 16);
                        }
                        fetch = (uint)(source[src] | (source[src + 1] << 8) | (source[src + 2] << 16));
                    }
                    else
                    {
                        fetch = (uint)(source[src] | (source[src + 1] << 8) | (source[src + 2] << 16) | (source[src + 3] << 24));
                    }
                    last_hashed = dst - 1;
                }
                else
                {
                    if (dst <= last_matchstart)
                    {
                        destination[dst] = source[src];
                        dst += 1;
                        src += 1;
                        cword_val = cword_val >> 1;

                        if (level == 1)
                        {
                            while (last_hashed < dst - 3)
                            {
                                last_hashed++;
                                int fetch2 = destination[last_hashed] | (destination[last_hashed + 1] << 8) | (destination[last_hashed + 2] << 16);
                                hash = ((fetch2 >> 12) ^ fetch2) & 4095;
                                hashtable[hash] = last_hashed;
                                hash_counter[hash] = 1;
                            }
                            fetch = (uint)(fetch >> 8 & 0xffff | source[src + 2] << 16);
                        }
                        else
                        {
                            fetch = (uint)(fetch >> 8 & 0xffff | source[src + 2] << 16 | source[src + 3] << 24);
                        }
                    }
                    else
                    {
                        while (dst <= size - 1)
                        {
                            if (cword_val == 1)
                            {
                                src += 4;
                                cword_val = 0x80000000;
                            }

                            destination[dst] = source[src];
                            dst++;
                            src++;
                            cword_val = cword_val >> 1;
                        }

                        return destination;
                    }
                }
            }
        }
    }
}
