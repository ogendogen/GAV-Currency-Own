using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExtractor.Models
{
    public class FinalOutput
    {
        public ApiLayerAPI ApiLayerAPI { get; set; }
        public ExChangeRatesAPI ExChangesRatesAPI { get; set; }
    }
}
