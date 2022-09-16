using System.Diagnostics;
using System.IO;
using YBiExtractor.Builder;

namespace YBiExtractor
{
    internal class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(File.Exists("./YBi.cfg"))
            {
                byte[] ybiBytes = Dec.Decrypt();

                ItemBuilder.YBBuffer = ybiBytes;
                ItemBuilder.ProcessYBItemData();

                MapBuilder.YBBuffer = ybiBytes;
                MapBuilder.ProcessYBMapData();
            }

            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
