using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend
{
    public class NBPRate
    {
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }
        public double Mid { get; set; } //for table A and B

        //for table C
        public double Bid { get; set; } 
        public double Ask { get; set; }
    }
}
