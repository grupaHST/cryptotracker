using Cryptotracker.Backend;
using Cryptotracker.Backend.NBP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptotrackerTests.Backend
{
    [TestClass]
    public class ExchangeRatesTest
    {
        [TestMethod]
        public void GetCurrencyDataTest()
        {
            object result = ExchangeRates.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.CHF);

            Assert.IsNotNull(result);
        }
    }
}
