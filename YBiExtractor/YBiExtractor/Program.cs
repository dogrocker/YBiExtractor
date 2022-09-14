using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using YBiExtractor.Templates;

namespace YBiExtractor
{
    internal class Program
    {
        /// <summary>
        /// 
        /// </summary>
        public static YbiReader Reader;

        public static List<byte[]> itemsByteArray = new List<byte[]>();
        public static List<byte[]> keyByteArray = new List<byte[]>();
        public static List<byte[]> skillsByteArray = new List<byte[]>();
        public static List<byte[]> qigongsByteArray = new List<byte[]>();
        public static List<byte[]> titlesByteArray = new List<byte[]>();
        public static List<byte[]> shields1ByteArray = new List<byte[]>();
        public static List<byte[]> shields2ByteArray = new List<byte[]>();
        public static List<byte[]> npcsByteArray = new List<byte[]>();
        public static List<byte[]> mapsByteArray = new List<byte[]>();
        public static List<byte[]> unknown1ByteArray = new List<byte[]>();
        public static List<byte[]> unknown2ByteArray = new List<byte[]>();

        public static List<ItemTemplate> items = new List<ItemTemplate>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(File.Exists("./YBi.cfg"))
            {
                byte[] ybiBytes = Dec.Decrypt();
                Reader = new YbiReader(ybiBytes);
                Reader.ReadD();
                Reader.ReadD();
                ReadData(852, 10000, ref itemsByteArray);
                foreach(var item in itemsByteArray)
                {
                    ReadItemDataToTemplate(item);
                }

                string json = JsonConvert.SerializeObject(items);
                File.WriteAllText("items.json", json.PrintJson());
            }

            Process.GetCurrentProcess().WaitForExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offsetSize"></param>
        /// <param name="count"></param>
        /// <param name="outBytes"></param>
        public static void ReadData(int offsetSize, int count, ref List<byte[]> outBytes)
        {
            for (int i = 0; i < count; i++)
            {
                byte[] item = Reader.ReadB(offsetSize);
                
                itemsByteArray.Add(item);
            }
        }

        public static void ReadItemDataToTemplate(byte[] data)
        {
            Console.WriteLine(data.FormatHex());
            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));

            using (MemoryStream Stream = new MemoryStream(data))
            {
                using (BinaryReader Reader = new BinaryReader(Stream))
                {
                    ItemTemplate template = new ItemTemplate();

                    template.Id = Reader.ReadInt32(); // 4
                    Reader.ReadInt32(); //4
                    template.Name = Encoding.GetEncoding("TIS-620").GetString(Reader.ReadBytes(64)).Replace("\0", ""); // 64
                    Reader.ReadByte(); // 1 ZX
                    template.Side = Reader.ReadByte(); 
                    template.Class = Reader.ReadByte();
                    Reader.ReadByte();
                    template.Level = Reader.ReadInt16();
                    template.JobLevel = Reader.ReadByte();
                    template.Gender = Reader.ReadByte();
                    template.Category = Reader.ReadByte();
                    template.SubCategory = Reader.ReadByte();
                    template.Weight = Reader.ReadInt16();
                    template.MaxAttack = Reader.ReadInt16();
                    template.MinAttack = Reader.ReadInt16();
                    template.Defense = Reader.ReadInt16();
                    template.Accuracy = Reader.ReadInt16();
                    Reader.ReadInt64();
                    template.Price = Reader.ReadInt32();

                    template.Description = Encoding.GetEncoding("TIS-620").GetString(data, 156, 256).Replace("\0", "");

                    items.Add(template);
                }
            }
        }
    }
}
