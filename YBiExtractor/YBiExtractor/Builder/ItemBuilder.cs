using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YBiExtractor.Templates;

namespace YBiExtractor.Builder
{
    public class ItemBuilder
    {
        public static string EncodingName = "TIS-620";
        public static byte[] YBBuffer;
        public static int BytesOfSeparation = 852;
        public static int ID_Offset = 0x0000035c;
        public static List<byte[]> ItemBufferList = new List<byte[]>();

        public static void ProcessYBItemData()
        {
            lock (YBBuffer)
            {
                while (BitConverter.ToInt64(YBBuffer, ID_Offset) != 0)
                {
                    long itemId = BitConverter.ToInt64(YBBuffer, ID_Offset);
                    byte[] temp = new byte[BytesOfSeparation];
                    Buffer.BlockCopy(YBBuffer, ID_Offset, temp, 0, BytesOfSeparation);
                    ItemBufferList.Add(temp);
                    ID_Offset += BytesOfSeparation;
                }
            }

            try
            {
                Directory.CreateDirectory("data/items/");
                foreach (byte[] data in ItemBufferList)
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
                            /*template.Category = */
                            Reader.ReadByte();
                            /*template.SubCategory = */
                            Reader.ReadByte();
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
