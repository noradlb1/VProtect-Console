namespace VProtect.Core.Services
{
    public class Murmur2
    {
        private const uint m = 0x5bd1e995;
        private const int r = 24;

        public unsafe static uint Hash(string data, uint seed)
        {
            fixed (char* input = data)
            {
                return Hash((byte*)input, (uint)data.Length * sizeof(char), seed);
            }
        }

        public unsafe static uint Hash(byte[] data, uint seed)
        {
            fixed (byte* input = &data[0])
            {
                return Hash(input, (uint)data.Length, seed);
            }
        }

        public unsafe static uint Hash(byte[] data, int offset, uint len, uint seed)
        {
            fixed (byte* input = &data[offset])
            {
                return Hash(input, len, seed);
            }
        }

        private unsafe static uint Hash(byte* data, uint len, uint seed)
        {
            uint h = seed ^ len;
            uint numberOfLoops = len >> 2; // div 4

            uint* realData = (uint*)data;
            while (numberOfLoops > 0)
            {
                uint k = *realData;

                k *= m;
                k ^= k >> r;
                k *= m;

                h *= m;
                h ^= k;

                realData++;
                numberOfLoops--;
            }
            var tail = (byte*)realData;
            switch (len & 3) // mod 4
            {
                case 3:
                    h ^= (uint)(tail[2] << 16);
                    h ^= (uint)(tail[1] << 8);
                    h ^= tail[0];
                    h *= m;
                    break;
                case 2:
                    h ^= (uint)(tail[1] << 8);
                    h ^= tail[0];
                    h *= m;
                    break;
                case 1:
                    h ^= tail[0];
                    h *= m;
                    break;
            }

            // Do a few final mixes of the hash to ensure the last few
            // bytes are well-incorporated.

            h ^= h >> 13;
            h *= m;
            h ^= h >> 15;

            return h;
        }
    }
}

