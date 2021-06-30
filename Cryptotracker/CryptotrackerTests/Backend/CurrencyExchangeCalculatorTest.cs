using Cryptotracker.Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptotrackerTests.Backend
{
    [TestClass]
    public class CurrencyExchangeCalculatorTest
    {
        [TestMethod]
        public void GetCurrentExchangeValueTest()
        {
            Assert.AreEqual(1, CurrencyExchangeCalculator.GetCurrentExchangeValue("PLN", ExchangePlatform.NBP.ToString()).Result);
        }

        [TestMethod]
        public void CalculateValueTest()
        {
            Assert.AreEqual((decimal)0.0, CurrencyExchangeCalculator.CalculateValue(1,null,1.25));
            Assert.AreEqual((decimal)0.0, CurrencyExchangeCalculator.CalculateValue(0, 5.25, 1.25));
            Assert.AreEqual((decimal)1.5, CurrencyExchangeCalculator.CalculateValue(1, 1.5, 1));
            Assert.AreEqual((decimal)-15, CurrencyExchangeCalculator.CalculateValue(-10, 1.5, 1));
        }
    }

}
