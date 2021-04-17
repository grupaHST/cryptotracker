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
            GenericCurrencyData result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.CHF, Convert.ToDateTime("2021-04-09"));

            Assert.IsNotNull(result);
            Assert.AreEqual(4.1265, result.Rates[0].Value);
            Assert.AreEqual(Convert.ToDateTime("2021-04-09"), result.Rates[0].Date);
            Assert.AreNotEqual(Convert.ToDateTime("2021-04-08"), result.Rates[0].Date);

            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.USD, Convert.ToDateTime("2021-04-06"), Convert.ToDateTime("2021-04-09"));
            Assert.AreEqual(4, result.Rates.Count);
            Assert.AreEqual(3.8611, result.Rates[1].Value);
            Assert.AreEqual(3.8208, result.Rates[3].Value);
            Assert.AreEqual(Convert.ToDateTime("2021-04-09"), result.Rates[3].Date);

            //DATE RANGE TESTS NBP
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.USD, Convert.ToDateTime("2021-04-14"), Convert.ToDateTime("2021-04-16"));
            Assert.AreEqual("USD", result.Code);
            Assert.AreEqual(3.8065, result.Rates[0].Value);
            Assert.AreEqual(3.8014, result.Rates[1].Value);
            Assert.AreEqual(3.7978, result.Rates[2].Value);
            Assert.AreEqual(3, result.Rates.Count);

            //NBP - START DATE ONLY
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.USD, Convert.ToDateTime("2021-04-14"));
            Assert.AreEqual("USD", result.Code);
            Assert.AreEqual(3.8065, result.Rates[0].Value);

            //NBP - DATA FROM LAST AVAILABLE DAY
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.USD, Convert.ToDateTime("2021-04-17"));
            Assert.IsNotNull(result);
            Assert.AreEqual(3.7978, result.Rates[0].Value);

            //DATE RANGE TESTS RATESAPI
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.RATES, CurrencyCode.USD, Convert.ToDateTime("2021-04-14"), Convert.ToDateTime("2021-04-16"));
            Assert.AreEqual("USD", result.Code);
            Assert.AreEqual(3.8061685055, result.Rates[0].Value);
            Assert.AreEqual(3.8056808688, result.Rates[1].Value);
            Assert.AreEqual(3.7968463207, result.Rates[2].Value);
            Assert.AreEqual(3, result.Rates.Count);

            //RATESAPI - START DATE ONLY
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.RATES, CurrencyCode.USD, Convert.ToDateTime("2021-04-14"));
            Assert.AreEqual("USD", result.Code);
            Assert.AreEqual(3.8061685055, result.Rates[0].Value);
        }
    }
}
