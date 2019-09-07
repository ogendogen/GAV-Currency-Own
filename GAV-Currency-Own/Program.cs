using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyExtractor;
using CurrencyExtractor.Models;
using CurrencyTransformator;

namespace GAV_Currency_Own
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start extracting process...");
            Console.ReadKey();

            try
            {
                Console.WriteLine("Starting extraction...");
                MediatedSchema mediatedSchema = Extractor.GetOutputFromWeb("EUR");
                Console.WriteLine("Mediated schema object deserialized");

                Console.WriteLine("Extraction successful");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured. Logging details...");
                Logger.LogException("ErrorLogs.log", e);
                Console.WriteLine("Logging finished. Press any key to stop application");
                Console.ReadKey();
            }
        }
    }
}
