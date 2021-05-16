using Cryptotracker.Backend.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend
{
    public class CurrencyDataModel
    {
        public string Code { get; set; }
        public List<RateModel> Rates { get; set; } = new List<RateModel>();
    }
}
