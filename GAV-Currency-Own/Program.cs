using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CurrencyExtractor;
using CurrencyExtractor.Models;
using CurrencyTransformator;
using CurrencyTransformator.Models;

namespace GAV_Currency_Own
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start extracting process...");
            Console.ReadKey();

            try
            {
                Console.WriteLine("Starting integration");
                Console.WriteLine("Extraction started");
                Console.WriteLine("Establishing connection...");
                List<MediatedSchema> mediatedSchemas = Extractor.GetAllFromWeb().ToList();
                Console.WriteLine("Mediated schema objects deserialized");

                Console.WriteLine("Extraction successful");
                Console.WriteLine("Starting transformation...");

                bool differentCurrency, differentDate;
                foreach (var mediated in mediatedSchemas)
                {
                    Validator.ValidateMediatedSchemaObject(mediated, out differentCurrency, out differentDate);
                    if (differentCurrency)
                    {
                        Console.WriteLine("Curriencies are different. Removing data");
                        continue;
                    }
                    if (differentDate)
                    {
                        Console.WriteLine("Dates are different. Removing data");
                        continue;
                    }

                    FinalOutput finalOutput = Transformator.TransformToOutput(mediated);
                    string serialized = Serializer.SerializeFinalOutput(finalOutput);
                    File.WriteAllText("../loader/transformed-" + mediated.API.@base + ".json", serialized);
                }

                Console.WriteLine("Transformation completed");
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
