using System;
using System.Collections;
using System.IO;

namespace YBiExtractor
{
    public class Dec
    {
        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] numArray = new byte[bits.Length / 8];
            bits.CopyTo((Array)numArray, 0);
            return numArray;
        }

        public static BitArray ByteArraytoBitArray(byte[] bytes)
        {
            return new BitArray(bytes);
        }

        public static byte[] Decrypt()
        {
            byte[] buffer;
            using (Stream stream = (Stream)File.OpenRead("./YBi.cfg"))
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
            }

            int startIndex = 0;
            while (startIndex < buffer.Length)
            {
                int src = BitConverter.ToInt32(buffer, startIndex);
                int num = 0 | Dec.MoveBit(src, 26, 0) | Dec.MoveBit(src, 31, 1) | Dec.MoveBit(src, 17, 2) | Dec.MoveBit(src, 10, 3) | Dec.MoveBit(src, 30, 4) | Dec.MoveBit(src, 16, 5) | Dec.MoveBit(src, 24, 6) | Dec.MoveBit(src, 2, 7) | Dec.MoveBit(src, 29, 8) | Dec.MoveBit(src, 8, 9) | Dec.MoveBit(src, 20, 10) | Dec.MoveBit(src, 15, 11) | Dec.MoveBit(src, 28, 12) | Dec.MoveBit(src, 11, 13) | Dec.MoveBit(src, 13, 14) | Dec.MoveBit(src, 4, 15) | Dec.MoveBit(src, 19, 16) | Dec.MoveBit(src, 23, 17) | Dec.MoveBit(src, 0, 18) | Dec.MoveBit(src, 12, 19) | Dec.MoveBit(src, 14, 20) | Dec.MoveBit(src, 27, 21) | Dec.MoveBit(src, 6, 22) | Dec.MoveBit(src, 18, 23) | Dec.MoveBit(src, 21, 24) | Dec.MoveBit(src, 3, 25) | Dec.MoveBit(src, 9, 26) | Dec.MoveBit(src, 7, 27) | Dec.MoveBit(src, 22, 28) | Dec.MoveBit(src, 1, 29) | Dec.MoveBit(src, 25, 30) | Dec.MoveBit(src, 5, 31);
                buffer[startIndex] = (byte)(num & (int)byte.MaxValue);
                buffer[startIndex + 1] = (byte)(num >> 8 & (int)byte.MaxValue);
                buffer[startIndex + 2] = (byte)(num >> 16 & (int)byte.MaxValue);
                buffer[startIndex + 3] = (byte)(num >> 24 & (int)byte.MaxValue);
                startIndex += 4;
            }

            return buffer;
            //using (FileStream fileStream = File.OpenWrite(OutPutFile))
            //    fileStream.Write(buffer, 0, buffer.Length);
        }

        private static int MoveBit(int src, int oldLoc, int newLoc)
        {
            return (src >> oldLoc & 1) << newLoc;
        }
    }
}
