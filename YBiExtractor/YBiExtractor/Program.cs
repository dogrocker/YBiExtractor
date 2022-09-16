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
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(File.Exists("./YBi.cfg"))
            {
                byte[] ybiBytes = Dec.Decrypt();

                ItemBuilder.YBBuffer = ybiBytes;
                ItemBuilder.ProcessYBItemData();
            }

            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
