using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.ExchangeRateHost
{
    public class ExchangeRateHostRate
    {
        public DateTime Date { get; set; }
        public Dictionary<string, double> ValuesDict { get; set; }
    }
}
