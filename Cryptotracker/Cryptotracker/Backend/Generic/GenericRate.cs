using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.Generic
{
    public class GenericRate
    {
        public double Low { get; set; } //for crypto
        public double High { get; set; } //for crypto
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}
