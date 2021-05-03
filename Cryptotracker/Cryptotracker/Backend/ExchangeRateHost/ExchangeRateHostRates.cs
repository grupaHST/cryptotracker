using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.ExchangeRateHost
{
    public class ExchangeRateHostRecord
    {
        public Dictionary<DateTime, ExchangeRateHostRates> Records { get; set; }
    }
    public class ExchangeRateHostRates
    {
        public Dictionary<string, double> Rates { get; set; }
    }
}
