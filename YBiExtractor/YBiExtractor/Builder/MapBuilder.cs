using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YBiExtractor.Templates;

namespace YBiExtractor.Builder
{
    public class MapBuilder
    {
        public static string EncodingName = "TIS-620";
        public static byte[] YBBuffer;
        public static int BytesOfSeparation = 744; // TH = 744, EN = ???
        public static int ID_Offset = 0x022BE2FC; // TH = 0x022BE2FC, EN = 0x0223BC40
        public static List<byte[]> MapBufferList = new List<byte[]>();
        public static List<MapTemplate> MapTemplates = new List<MapTemplate>();

        public static void ProcessYBMapData()
        {
            lock (YBBuffer)
            {
                while (BitConverter.ToInt64(YBBuffer, ID_Offset) != 0)
                {
                    long itemId = BitConverter.ToInt64(YBBuffer, ID_Offset);
                    byte[] temp = new byte[BytesOfSeparation];
                    Buffer.BlockCopy(YBBuffer, ID_Offset, temp, 0, BytesOfSeparation);
                    MapBufferList.Add(temp);
                    ID_Offset += BytesOfSeparation;
                }
            }

            try
            {
                foreach (byte[] data in MapBufferList)
                {
                    using (MemoryStream Stream = new MemoryStream(data))
                    {
                        using (BinaryReader Reader = new BinaryReader(Stream))
                        {
                            MapTemplate template = new MapTemplate();
                            template.Id = Reader.ReadInt32();
                            template.Name = Encoding.GetEncoding("TIS-620").GetString(Reader.ReadBytes(64)).Replace("\0", "");
                            template.X1 = Reader.ReadSingle();
                            template.Z1 = Reader.ReadSingle();
                            template.Y1 = Reader.ReadSingle();
                            template.X2 = Reader.ReadSingle();
                            template.Z2 = Reader.ReadSingle();
                            template.Y2 = Reader.ReadSingle();

                            MapTemplates.Add(template);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                string json = JsonConvert.SerializeObject(MapTemplates);
                File.WriteAllText($"data/maps.json", json.PrintJson());
                Console.WriteLine($"Write file: data/maps.json");
            }
        }
    }
}
