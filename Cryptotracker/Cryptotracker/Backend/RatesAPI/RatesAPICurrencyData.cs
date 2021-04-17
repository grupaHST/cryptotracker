using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.Rates
{
    public class RatesAPICurrencyData
    {
        public string Base { get; set; }
        public Dictionary<string, double> Rates { get; set;}
        public DateTime Date { get; set; }
    }
}
