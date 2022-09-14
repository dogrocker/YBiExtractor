using System;
using System.IO;
using System.Text;

namespace YBiExtractor
{
    public class YbiReader
    {
        private BinaryReader Reader;

        public YbiReader(byte[] YbiBytes)
        {
            Reader = new BinaryReader(new MemoryStream(YbiBytes));
        }

        public int ReadD()
        {
            try
            {
                return Reader.ReadInt32();
            }
            catch (Exception)
            {
                Console.WriteLine("Missing D for: {0}", GetType());
            }
            return 0;
        }

        public int ReadC()
        {
            try
            {
                return Reader.ReadByte() & 0xFF;
            }
            catch (Exception)
            {
                Console.WriteLine("Missing C for: {0}", GetType());
            }
            return 0;
        }

        public int ReadH()
        {
            try
            {
                return Reader.ReadInt16() & 0xFFFF;
            }
            catch (Exception)
            {
                Console.WriteLine("Missing H for: {0}", GetType());
            }
            return 0;
        }

        public double ReadDf()
        {
            try
            {
                return Reader.ReadDouble();
            }
            catch (Exception)
            {
                Console.WriteLine("Missing DF for: {0}", GetType());
            }
            return 0;
        }

        public float ReadF()
        {
            try
            {
                return Reader.ReadSingle();
            }
            catch (Exception)
            {
                Console.WriteLine("Missing F for: {0}", GetType());
            }
            return 0;
        }

        public long ReadQ()
        {
            try
            {
                return Reader.ReadInt64();
            }
            catch (Exception)
            {
                Console.WriteLine("Missing Q for: {0}", GetType());
            }
            return 0;
        }

        public String ReadS()
        {
            Encoding encoding = Encoding.Default;
            String result = "";
            try
            {
                int length = ReadH();
                byte[] bytes = ReadB(length);
                result = encoding.GetString(bytes);
            }
            catch (Exception)
            {
                Console.WriteLine("Missing S for: {0}", GetType());
            }
            return result;
        }

        public String ReadS(int length)
        {
            Encoding encoding = Encoding.Default;
            String result = "";
            try
            {
                byte[] bytes = ReadB(length);
                result = encoding.GetString(bytes);
            }
            catch (Exception)
            {
                Console.WriteLine("Missing S for: {0}", GetType());
            }
            return result;
        }

        public byte[] ReadB(int length)
        {
            byte[] result = new byte[length];
            try
            {
                Reader.Read(result, 0, length);
            }
            catch (Exception)
            {
                Console.WriteLine("Missing byte[] for: {0}", GetType());
            }
            return result;
        }
    }
}
