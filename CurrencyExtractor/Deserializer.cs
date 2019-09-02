using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyExtractor.Models;
using Newtonsoft.Json;

namespace CurrencyExtractor
{
    public static class Deserializer
    {
        public static FinalOutput DeserializeToFinalOutput(string rawApiLayer, string rawExChangeRates)
        {
            FinalOutput finalOutput = new FinalOutput();
            finalOutput.ApiLayerAPI = JsonConvert.DeserializeObject<ApiLayerAPI>(rawApiLayer);
            finalOutput.ExChangesRatesAPI = JsonConvert.DeserializeObject<ExChangeRatesAPI>(rawExChangeRates);

            return finalOutput;
        }
    }
}
