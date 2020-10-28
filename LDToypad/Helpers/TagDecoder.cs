using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LDToypad
{
    public class TagDecoder
    {
        /// <summary>
        /// Key for TEA (see https://en.wikipedia.org/wiki/Tiny_Encryption_Algorithm for more about TEA).
        /// </summary>
        private static byte[] teakey = { 0x55, 0xFE, 0xF6, 0xB0, 0x62, 0xBF, 0x0B, 0x41,
            0xC9, 0xB3, 0x7C, 0xB4, 0x97, 0x3E, 0x29, 0x7B };

        /// <summary>
        /// Enciphers the specified v0 and v1.
        /// </summary>
        /// <param name="v0">The v0.</param>
        /// <param name="v1">The v1.</param>
        /// <returns>encrypted data</returns>
        private static uint[] encipher(uint v0, uint v1)
		{
			uint y = v0;
			uint z = v1;
			uint sum = 0;
			uint delta = 0x9E3779B9;
			uint a = readUInt32LE(teakey, 0);
			uint b = readUInt32LE(teakey, 4);
			uint c = readUInt32LE(teakey, 8);
			uint d = readUInt32LE(teakey, 12);
			uint n = 32;

			while (n-- > 0)
			{
				sum += delta;
				y += (z << 4) + a ^ z + sum ^ (z >> 5) + b;
				z += (y << 4) + c ^ y + sum ^ (y >> 5) + d;
			}
			return new uint[] { y, z };
		}

        /// <summary>
        /// Deciphers the specified v0 and v1.
        /// </summary>
        /// <param name="v0">The v0.</param>
        /// <param name="v1">The v1.</param>
        /// <returns>decrypted data</returns>
        private static uint[] decipher(uint v0, uint v1)
        {

            uint y = v0;
            uint z = v1;
            uint sum = 0xC6EF3720;
            uint delta = 0x9E3779B9;
            uint a = readUInt32LE(teakey, 0);
            uint b = readUInt32LE(teakey, 4);
            uint c = readUInt32LE(teakey, 8);
            uint d = readUInt32LE(teakey, 12);
            uint n = 32;

            while (n-- > 0)
            {
                z -= (y << 4) + c ^ y + sum ^ (y >> 5) + d;
                y -= (z << 4) + a ^ z + sum ^ (z >> 5) + b;
                sum -= delta;
            }

            return new uint[] { y, z };
        }

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>encoded data</returns>
        public static byte[] Encode(byte[] data)
        {
            byte[] data2 = new byte[8];
            data2[0] = (byte)readUInt32LE(data, 0);
            data2[1] = (byte)readUInt32LE(data, 4);

            byte[] bp = data2;
            uint r0 = (uint)(bp[0] << 24 | (bp[1] & 0xff) << 16 | (bp[2] & 0xff) << 8 | bp[3] & 0xff);
            uint r1 = (uint)(bp[4] << 24 | (bp[5] & 0xff) << 16 | (bp[6] & 0xff) << 8 | bp[7] & 0xff);
            uint[] r = encipher(r0, r1);

            return writeUInt32LE(r[0]).Concat(writeUInt32LE(r[1])).ToArray();
        }

        /// <summary>
        /// Decodes the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private static byte[] decodeData(byte[] data)
        {

            uint d1 = readUInt32LE(data, 0);
            uint d2 = readUInt32LE(data, 4);

            uint[] tmp = decipher(d1, d2);

            return writeUInt32LE(tmp[0]).Concat(writeUInt32LE(tmp[1])).ToArray();
        }

        /// <summary>
        /// Decodes a character tag.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Character DecodeCharacter(byte[] data)
        {
            byte[] dataDecod = decodeData(data);

            uint id = readUInt32LE(dataDecod, 0);

            if (Enum.IsDefined(typeof(Character), (int)id)) return (Character)id;
            else return Character.Unrecognized;
        }

        /// <summary>
        /// Decodes a vehicle tag.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Character DecodeVehicle(byte[] data)
        {
            uint id = readUInt16LE(data, 0);
            if (Enum.IsDefined(typeof(Character), (int)id)) return (Character)id;
            else return Character.Unrecognized;
        }

        /// <summary>
        /// Read int 16 bits Little Endian
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="pointer">The pointer.</param>
        /// <returns></returns>
        private static uint readUInt16LE(byte[] bytes, int pointer)
        {
            using (MemoryStream ms = new MemoryStream(bytes, pointer, 2))
            using (BinaryReader br = new BinaryReader(ms))
                return br.ReadUInt16();
        }

        /// <summary>
        /// Read int 32 bits Little Endian
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="pointer">The pointer.</param>
        /// <returns></returns>
        private static uint readUInt32LE(byte[] bytes, int pointer)
        {
            using (MemoryStream ms = new MemoryStream(bytes, pointer, 4))
            using (BinaryReader br = new BinaryReader(ms))
                return br.ReadUInt32();
        }

        /// <summary>
        /// Write a value int 32 bits Little Endian to the buffer.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <param name="buf">The buffer.</param>
        private static byte[] writeUInt32LE(uint v)
        {
            return new byte[] {
                (byte)(v & 0x000000ff),
                (byte)((v & 0x0000ff00) >> 8),
                (byte)((v & 0x00ff0000) >> 16),
                (byte)((v & 0xff000000) >> 24),
            };
        }
    }
}
