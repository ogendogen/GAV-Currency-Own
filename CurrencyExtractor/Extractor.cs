using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using CurrencyExtractor.Models;
using Newtonsoft.Json;

namespace CurrencyExtractor
{
    public static class Extractor
    {
        public static FinalOutput GetOutputFromFile(string apiLayerPath, string exChangeRatesPath)
        {
            string rawApiLayer = File.ReadAllText(apiLayerPath);
            string rawExChangeRates = File.ReadAllText(exChangeRatesPath);

            return Deserializer.DeserializeToFinalOutput(rawApiLayer, rawExChangeRates);
        }

        public static FinalOutput GetOutputFromWeb()
        {
            WebClient webClient = new WebClient();

            string rawApiLayer = webClient.DownloadString("http://www.apilayer.net/api/live?access_key=aa06bb8220d39379e3c4da759764407a");
            string rawExChangeRates = webClient.DownloadString("https://api.exchangeratesapi.io/latest");

            return Deserializer.DeserializeToFinalOutput(rawApiLayer, rawExChangeRates);
        }
    }
}
