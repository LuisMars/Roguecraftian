using Roguecraft.Engine.Random;

namespace Warband.Engines.Random
{
    public class XXHash : HashFunction
    {
        private const uint PRIME32_1 = 2654435761U;
        private const uint PRIME32_2 = 2246822519U;
        private const uint PRIME32_3 = 3266489917U;
        private const uint PRIME32_4 = 668265263U;
        private const uint PRIME32_5 = 374761393U;
        private readonly uint _seed;

        public XXHash(int seed)
        {
            _seed = (uint)seed;
        }

        public XXHash()
        {
            _seed = (uint)DateTime.Now.Ticks;
        }

        public uint GetHash(byte[] buf)
        {
            uint h32;
            int index = 0;
            int len = buf.Length;

            if (len >= 16)
            {
                int limit = len - 16;
                uint v1 = _seed + PRIME32_1 + PRIME32_2;
                uint v2 = _seed + PRIME32_2;
                uint v3 = _seed + 0;
                uint v4 = _seed - PRIME32_1;

                do
                {
                    v1 = CalcSubHash(v1, buf, index);
                    index += 4;
                    v2 = CalcSubHash(v2, buf, index);
                    index += 4;
                    v3 = CalcSubHash(v3, buf, index);
                    index += 4;
                    v4 = CalcSubHash(v4, buf, index);
                    index += 4;
                } while (index <= limit);

                h32 = RotateLeft(v1, 1) + RotateLeft(v2, 7) + RotateLeft(v3, 12) + RotateLeft(v4, 18);
            }
            else
            {
                h32 = _seed + PRIME32_5;
            }

            h32 += (uint)len;

            while (index <= len - 4)
            {
                h32 += BitConverter.ToUInt32(buf, index) * PRIME32_3;
                h32 = RotateLeft(h32, 17) * PRIME32_4;
                index += 4;
            }

            while (index < len)
            {
                h32 += buf[index] * PRIME32_5;
                h32 = RotateLeft(h32, 11) * PRIME32_1;
                index++;
            }

            h32 ^= h32 >> 15;
            h32 *= PRIME32_2;
            h32 ^= h32 >> 13;
            h32 *= PRIME32_3;
            h32 ^= h32 >> 16;

            return h32;
        }

        public uint GetHash(params uint[] buf)
        {
            uint h32;
            int index = 0;
            int len = buf.Length;

            if (len >= 4)
            {
                int limit = len - 4;
                uint v1 = _seed + PRIME32_1 + PRIME32_2;
                uint v2 = _seed + PRIME32_2;
                uint v3 = _seed + 0;
                uint v4 = _seed - PRIME32_1;

                do
                {
                    v1 = CalcSubHash(v1, buf[index]);
                    index++;
                    v2 = CalcSubHash(v2, buf[index]);
                    index++;
                    v3 = CalcSubHash(v3, buf[index]);
                    index++;
                    v4 = CalcSubHash(v4, buf[index]);
                    index++;
                } while (index <= limit);

                h32 = RotateLeft(v1, 1) + RotateLeft(v2, 7) + RotateLeft(v3, 12) + RotateLeft(v4, 18);
            }
            else
            {
                h32 = _seed + PRIME32_5;
            }

            h32 += (uint)len * 4;

            while (index < len)
            {
                h32 += buf[index] * PRIME32_3;
                h32 = RotateLeft(h32, 17) * PRIME32_4;
                index++;
            }

            h32 ^= h32 >> 15;
            h32 *= PRIME32_2;
            h32 ^= h32 >> 13;
            h32 *= PRIME32_3;
            h32 ^= h32 >> 16;

            return h32;
        }

        public override uint GetHash(params int[] buf)
        {
            uint h32;
            int index = 0;
            int len = buf.Length;

            if (len >= 4)
            {
                int limit = len - 4;
                uint v1 = (uint)_seed + PRIME32_1 + PRIME32_2;
                uint v2 = (uint)_seed + PRIME32_2;
                uint v3 = (uint)_seed + 0;
                uint v4 = (uint)_seed - PRIME32_1;

                do
                {
                    v1 = CalcSubHash(v1, (uint)buf[index]);
                    index++;
                    v2 = CalcSubHash(v2, (uint)buf[index]);
                    index++;
                    v3 = CalcSubHash(v3, (uint)buf[index]);
                    index++;
                    v4 = CalcSubHash(v4, (uint)buf[index]);
                    index++;
                } while (index <= limit);

                h32 = RotateLeft(v1, 1) + RotateLeft(v2, 7) + RotateLeft(v3, 12) + RotateLeft(v4, 18);
            }
            else
            {
                h32 = (uint)_seed + PRIME32_5;
            }

            h32 += (uint)len * 4;

            while (index < len)
            {
                h32 += (uint)buf[index] * PRIME32_3;
                h32 = RotateLeft(h32, 17) * PRIME32_4;
                index++;
            }

            h32 ^= h32 >> 15;
            h32 *= PRIME32_2;
            h32 ^= h32 >> 13;
            h32 *= PRIME32_3;
            h32 ^= h32 >> 16;

            return h32;
        }

        public override uint GetHash(int buf)
        {
            uint h32 = (uint)_seed + PRIME32_5;
            h32 += 4U;
            h32 += (uint)buf * PRIME32_3;
            h32 = RotateLeft(h32, 17) * PRIME32_4;
            h32 ^= h32 >> 15;
            h32 *= PRIME32_2;
            h32 ^= h32 >> 13;
            h32 *= PRIME32_3;
            h32 ^= h32 >> 16;
            return h32;
        }

        private static uint CalcSubHash(uint value, byte[] buf, int index)
        {
            uint read_value = BitConverter.ToUInt32(buf, index);
            value += read_value * PRIME32_2;
            value = RotateLeft(value, 13);
            value *= PRIME32_1;
            return value;
        }

        private static uint CalcSubHash(uint value, uint read_value)
        {
            value += read_value * PRIME32_2;
            value = RotateLeft(value, 13);
            value *= PRIME32_1;
            return value;
        }

        private static uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }
    }
}