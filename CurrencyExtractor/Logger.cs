using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CurrencyExtractor
{
    public static class Logger
    {
        public static void LogException(string path, Exception ex)
        {
            try
            {
                File.AppendAllText(path, DateTime.Now.ToString("0:MM/dd/yy H:mm:ss") + " Message: " + ex.Message + "\r\n" + "StackTrace: " + ex.StackTrace);
            }
            catch (Exception innerEx)
            {
                Console.WriteLine("Inner exception during generating logs. Aborting application. Message: " + innerEx.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
