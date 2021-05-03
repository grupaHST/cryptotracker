using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.ExchangeRateHost
{
    public class ExchangeRateHostData
    {
        public string Base { get; set; }
        public List<ExchangeRateHostRecord> Records { get; set; }
    }
}
