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
    public class ExchangeRatesHandlerTest
    {
        [TestMethod]
        public async Task GetCurrencyDataTest()
        {
            object result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.CHF);

            Assert.IsNotNull(result);
            Assert.AreEqual(4.1265, (result as GenericCurrencyData).Rates[0].Value);
            Assert.AreEqual(Convert.ToDateTime("2021-04-09"), (result as GenericCurrencyData).Rates[0].Date);
            Assert.AreNotEqual(Convert.ToDateTime("2021-04-08"), (result as GenericCurrencyData).Rates[0].Date);

            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.USD, Convert.ToDateTime("2021-04-06"), Convert.ToDateTime("2021-04-09"));
            Assert.AreEqual(4, (result as GenericCurrencyData).Rates.Count);
            Assert.AreEqual(3.8611, (result as GenericCurrencyData).Rates[1].Value);
            Assert.AreEqual(3.8208, (result as GenericCurrencyData).Rates[3].Value);
            Assert.AreEqual(Convert.ToDateTime("2021-04-09"), (result as GenericCurrencyData).Rates[3].Date);
        }
    }
}
