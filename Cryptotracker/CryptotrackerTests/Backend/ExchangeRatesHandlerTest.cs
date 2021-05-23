using Cryptotracker;
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
            // TESTING API KEYS
            ExchangeRatesHandler.BitfinexAPIKey = "mDqhNh8HLJmbuBdCSLktNXhpsaqtA4FSdAOutllUoWh";
            ExchangeRatesHandler.BitfinexAPISecret = "jv3bsJhv6L0lUYPHbO1KXsPPfzcP5rrcfRjt80QO0t8";
            ExchangeRatesHandler.BinanceAPIKey = "ZD4viGXGIAdLEOUN5vOIF4LQfcBQhrcdbX8r9oUd1ACo29kpd35G0g6uXl7nLRnh";
            ExchangeRatesHandler.BinanceAPISecret = "YdR3dMvkcEtGboOm4SnfOwnYRR6qhmTFCPfPcXO0yerYi1LCO2QhCmP5tUj9c8He";

            CurrencyDataModel result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.CHF, Convert.ToDateTime("2021-04-09"));

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

            //NBP - UNAVAILABLE DATA
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.USD, Convert.ToDateTime("2021-04-17"));
            Assert.IsNull(result);

            //NBP - TABLE B
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.LSL, Convert.ToDateTime("2021-04-25"));
            Assert.IsNull(result);

            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.NBP, CurrencyCode.LSL, Convert.ToDateTime("2021-04-21"));
            Assert.IsNotNull(result);
            Assert.AreEqual("LSL", result.Code);
            Assert.AreEqual(Convert.ToDateTime("2021-04-21"), result.Rates.First().Date);
            Assert.AreEqual(0.2651, result.Rates.First().Value);

            //DATE RANGE TESTS YAHOO
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.YAHOO, CurrencyCode.USD, Convert.ToDateTime("2021-04-14"), Convert.ToDateTime("2021-04-16"));
            Assert.AreEqual("USD", result.Code);
            Assert.AreEqual(3.806945, result.Rates[0].Value);
            Assert.AreEqual(3.80178, result.Rates[1].Value);
            Assert.AreEqual(3.7968, result.Rates[2].Value);
            Assert.AreEqual(3, result.Rates.Count);

            //YAHOO - START DATE ONLY
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.YAHOO, CurrencyCode.USD, Convert.ToDateTime("2021-04-14"));
            Assert.AreEqual("USD", result.Code);
            Assert.AreEqual(3.806945, result.Rates[0].Value);


            //EXCHANGERATES.HOST - START DATE ONLY
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.EXCHANGERATE_HOST, CurrencyCode.USD, Convert.ToDateTime("2021-04-23"));
            Assert.AreEqual("USD", result.Code);
            Assert.AreEqual(3.76605, result.Rates.First().Value);

            //EXCHANGERATES.HOST - DATE RANGE TESTS
            result = await ExchangeRatesHandler.GetCurrencyData(ExchangePlatform.EXCHANGERATE_HOST, CurrencyCode.CHF, Convert.ToDateTime("2021-04-10"), Convert.ToDateTime("2021-04-23"));
            Assert.AreEqual("CHF", result.Code);
            Assert.AreEqual(Convert.ToDateTime("2021-04-10"), result.Rates[0].Date);
            Assert.AreEqual(4.128723, result.Rates[0].Value);
            Assert.AreEqual(Convert.ToDateTime("2021-04-11"), result.Rates[1].Date);
            Assert.AreEqual(4.11611, result.Rates[1].Value);
            Assert.AreEqual(4.124458, result.Rates[2].Value);
            Assert.AreEqual(4.150657, result.Rates[3].Value);
            Assert.AreEqual(4.118434, result.Rates[4].Value);
            Assert.AreEqual(4.124598, result.Rates[5].Value);
            Assert.AreEqual(4.120131, result.Rates[6].Value);
            Assert.AreEqual(4.120737, result.Rates[7].Value);
            Assert.AreEqual(4.121566, result.Rates[8].Value);
            Assert.AreEqual(4.131485, result.Rates[9].Value);
            Assert.AreEqual(4.130249, result.Rates[10].Value);
            Assert.AreEqual(4.129144, result.Rates[11].Value);
            Assert.AreEqual(Convert.ToDateTime("2021-04-22"), result.Rates[12].Date);
            Assert.AreEqual(4.142222, result.Rates[12].Value);
            Assert.AreEqual(Convert.ToDateTime("2021-04-23"), result.Rates[13].Date);
            Assert.AreEqual(4.120364, result.Rates[13].Value);

            //CRYPTO
            //BINANCE - DATE RANGE TESTS
            result = await ExchangeRatesHandler.GetCryptocurrencyData(CryptoExchangePlatform.BINANCE, CryptocurrencyCode.BTC, CryptoInterval.ONE_DAY, Convert.ToDateTime("2021-05-07"), Convert.ToDateTime("2021-05-08"));
            Assert.AreEqual(Convert.ToDateTime("2021-05-07"), result.Rates[0].Date);
            Assert.AreEqual(55295.09, result.Rates[0].Low);
            Assert.AreEqual(58733.43, result.Rates[0].High);
            Assert.AreEqual((result.Rates[0].High + result.Rates[0].Low) / 2, result.Rates[0].Value, 0.1);

            Assert.AreEqual(Convert.ToDateTime("2021-05-08"), result.Rates[1].Date);
            Assert.AreEqual(56970.00, result.Rates[1].Low);
            Assert.AreEqual(59561.41, result.Rates[1].High);
            Assert.AreEqual((result.Rates[1].High + result.Rates[1].Low) / 2, result.Rates[1].Value);

            //BINANCE - START TIME ONLY - START DATE -> TODAY
            result = await ExchangeRatesHandler.GetCryptocurrencyData(CryptoExchangePlatform.BINANCE, CryptocurrencyCode.BTC, CryptoInterval.ONE_DAY, Convert.ToDateTime("2021-05-07"));
            Assert.AreEqual(true, result.Rates.Count() > 2);
            Assert.AreEqual(Convert.ToDateTime("2021-05-07"), result.Rates[0].Date);
            Assert.AreEqual(55295.09, result.Rates[0].Low);
            Assert.AreEqual(58733.43, result.Rates[0].High);
            Assert.AreEqual((result.Rates[0].High + result.Rates[0].Low) / 2, result.Rates[0].Value, 0.1);

            //BINANCE - 24H VALUE, ONLY INTERVAL PROVIDED (to be ignored)
            result = await ExchangeRatesHandler.GetCryptocurrencyData(CryptoExchangePlatform.BINANCE, CryptocurrencyCode.BTC, CryptoInterval.ONE_HOUR);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Rates.Count());

            //BITFINEX - DATE RANGE TESTS
            result = await ExchangeRatesHandler.GetCryptocurrencyData(CryptoExchangePlatform.BITFINEX, CryptocurrencyCode.BTC, CryptoInterval.ONE_DAY, Convert.ToDateTime("2021-05-07"), Convert.ToDateTime("2021-05-08"));
            Assert.AreEqual(Convert.ToDateTime("2021-05-07"), result.Rates[0].Date);
            Assert.AreEqual(55300, result.Rates[0].Low);
            Assert.AreEqual(58635, result.Rates[0].High);
            Assert.AreEqual((result.Rates[0].High + result.Rates[0].Low) / 2, result.Rates[0].Value, 0.1);

            Assert.AreEqual(Convert.ToDateTime("2021-05-08"), result.Rates[1].Date);
            Assert.AreEqual(56939, result.Rates[1].Low);
            Assert.AreEqual(59450, result.Rates[1].High);
            Assert.AreEqual((result.Rates[1].High + result.Rates[1].Low) / 2, result.Rates[1].Value);

            //BITFINEX - START TIME ONLY - START DATE -> TODAY
            result = await ExchangeRatesHandler.GetCryptocurrencyData(CryptoExchangePlatform.BITFINEX, CryptocurrencyCode.BTC, CryptoInterval.ONE_DAY, Convert.ToDateTime("2021-05-07"));
            Assert.AreEqual(true, result.Rates.Count() > 2);
            Assert.AreEqual(Convert.ToDateTime("2021-05-07"), result.Rates[0].Date);
            Assert.AreEqual(55300, result.Rates[0].Low);
            Assert.AreEqual(58635, result.Rates[0].High);
            Assert.AreEqual((result.Rates[0].High + result.Rates[0].Low) / 2, result.Rates[0].Value, 0.1);

            //BITFINEX - 24H VALUE, ONLY INTERVAL PROVIDED (to be ignored)
            result = await ExchangeRatesHandler.GetCryptocurrencyData(CryptoExchangePlatform.BITFINEX, CryptocurrencyCode.BTC, CryptoInterval.ONE_HOUR);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Rates.Count());
        }
    }
}
