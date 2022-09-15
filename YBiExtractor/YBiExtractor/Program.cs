using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        //public static List<ItemTemplate> items = new List<ItemTemplate>();

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
            using (MemoryStream Stream = new MemoryStream(data))
            {
                using (BinaryReader Reader = new BinaryReader(Stream))
                {
                    ItemTemplate template = new ItemTemplate();

                    template.Id = Reader.ReadInt32();
                    Reader.ReadInt32();
                    template.Name = Encoding.GetEncoding("TIS-620").GetString(Reader.ReadBytes(64)).Replace("\0", ""); // 64
                    template.Zx = Reader.ReadByte(); // ZX
                    Reader.ReadByte();
                    template.Job = Reader.ReadByte();
                    template.Side1 = Reader.ReadByte();
                    template.Level = Reader.ReadInt16();
                    template.JobLevel = Reader.ReadByte();
                    template.Gender = Reader.ReadByte();
                    /*template.Category = */Reader.ReadByte();
                    /*template.SubCategory = */Reader.ReadByte();
                    template.Weight = Reader.ReadInt16();
                    template.MaxAttack = Reader.ReadInt16();
                    template.MinAttack = Reader.ReadInt16();
                    template.Defense = Reader.ReadInt16();
                    template.Accuracy = Reader.ReadInt16();
                    Reader.ReadInt64();
                    template.Price = Reader.ReadInt32(); //100 101 102 103

                    Reader.ReadBytes(4); // 104 105 106 107

                    template.SalePrice = Reader.ReadInt32(); // 108 109 110 111

                    Reader.ReadByte(); // 112

                    template.Unk2 = Reader.ReadInt16(); // 113 114 115 116

                    Reader.ReadBytes(39); // 117 ~ 155

                    template.Description = Encoding.GetEncoding("TIS-620").GetString(data, 156, 256).Replace("\0", "");

                    template.Wx = BitConverter.ToInt32(data, 412);
                    template.Wxjd = BitConverter.ToInt32(data, 416);

                    string json = JsonConvert.SerializeObject(template);
                    File.WriteAllText($"data/items/{template.Id}.json", json.PrintJson());
                    Console.WriteLine($"Write file: data/items/{template.Id}.json");
                    if(template.Id == 700117)
                    {
                        Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
                    }
                }
            }
        }
    }
}
