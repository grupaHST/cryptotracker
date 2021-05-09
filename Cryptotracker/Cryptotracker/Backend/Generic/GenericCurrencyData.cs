using Cryptotracker.Backend.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend
{
    public class GenericCurrencyData
    {
        public string Code { get; set; }
        public List<GenericRate> Rates { get; set; } = new List<GenericRate>();
    }
}
