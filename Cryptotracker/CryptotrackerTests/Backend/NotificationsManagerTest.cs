using Cryptotracker.Backend;
using Cryptotracker.Backend.Notifications;
using Cryptotracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptotrackerTests.Backend
{
    [TestClass]
    public class NotificationsManagerTest
    {
        private static double CryptoCurrentPrice = 0;

        private static CurrencyDataModel CurrencyDataModel = new();

        private static List<NotificationModel> notifications = new();

        public static async Task<double> GetCryptoCurrentPriceMock(CryptoExchangePlatform cryptoPlatform, CryptocurrencyCode cryptocurrencyCode)
        {
            return CryptoCurrentPrice;
        }
        public static async Task<CurrencyDataModel> GetCurrencyDataMock(ExchangePlatform currencyPlatform, CurrencyCode currencyCode, DateTime? startTime = null, DateTime? endTime = null)
        {
            return CurrencyDataModel;
        }
        public static void OnNotificationOccurence(object? sender, List<NotificationModel> notificationsToDisplay)
        {
            notifications = notificationsToDisplay;
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            // TESTING API KEYS
            ExchangeRatesHandler.BitfinexAPIKey = "mDqhNh8HLJmbuBdCSLktNXhpsaqtA4FSdAOutllUoWh";
            ExchangeRatesHandler.BitfinexAPISecret = "jv3bsJhv6L0lUYPHbO1KXsPPfzcP5rrcfRjt80QO0t8";
            ExchangeRatesHandler.BinanceAPIKey = "ZD4viGXGIAdLEOUN5vOIF4LQfcBQhrcdbX8r9oUd1ACo29kpd35G0g6uXl7nLRnh";
            ExchangeRatesHandler.BinanceAPISecret = "YdR3dMvkcEtGboOm4SnfOwnYRR6qhmTFCPfPcXO0yerYi1LCO2QhCmP5tUj9c8He";

            NotificationManager.Init(ExchangeRatesHandler.GetCryptoCurrentPrice, ExchangeRatesHandler.GetFIATCurrencyData);

            NotificationManager.EventHandler = OnNotificationOccurence;

            //BINANCE
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BINANCE, 20000, Comparison.GREATER_THAN));
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.ETH, CryptoExchangePlatform.BINANCE, 1500, Comparison.GREATER_THAN_OR_EQUAL));
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BINANCE, 1000000, Comparison.GREATER_THAN)); //FALSE
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.ETH, CryptoExchangePlatform.BINANCE, 1000000, Comparison.LESS_THAN_OR_EQUAL));

            //BITFINEX
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BITFINEX, 20000, Comparison.GREATER_THAN));
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.ETH, CryptoExchangePlatform.BITFINEX, 1000, Comparison.GREATER_THAN));
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BITFINEX, 10000, Comparison.GREATER_THAN));

            await NotificationManager.Update();

            //BINANCE
            Assert.AreEqual(true, notifications[0].Threshold == 20000);
            Assert.AreEqual(true, notifications[0].CurrentValue > 20000);
            Assert.AreEqual(true, notifications[1].Threshold == 1500);
            Assert.AreEqual(false, (notifications[2].Threshold == 1000000 && notifications[2].CryptocurrencyCode == CryptocurrencyCode.BTC));
            Assert.AreEqual(true, notifications[2].Threshold == 1000000);
            Assert.AreEqual(true, notifications[2].CurrentValue < 1000000);

            //BITFINEX
            Assert.AreEqual(true, notifications[3].CurrentValue > 20000);
            Assert.AreEqual(true, notifications[3].Threshold == 20000);
            Assert.AreEqual(true, notifications[4].CurrentValue > 1000);
            Assert.AreEqual(true, notifications[4].Threshold == 1000);
            Assert.AreEqual(true, notifications[5].CurrentValue > 10000);
            Assert.AreEqual(true, notifications[5].Threshold == 10000);

            NotificationManager.Clear();
            notifications.Clear();

            ///MOCKED VALUES - SCENARIO 1
            NotificationManager.Init(GetCryptoCurrentPriceMock, GetCurrencyDataMock);

            //SHOW ONCE TEST
            CryptoCurrentPrice = 20000;
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BINANCE, 20000, Comparison.EQUAL));

            await NotificationManager.Update();
            Assert.AreEqual(true, notifications[0].Threshold == 20000); //SHOWN 
            await NotificationManager.Update();
            Assert.AreEqual(false, notifications.Any()); //NO NOTIFICATIONS (WAS ALREADY SHOWN)

            //BREAKING THE CONDITION
            CryptoCurrentPrice = 20001;
            await NotificationManager.Update();
            Assert.AreEqual(false, notifications.Any()); //SHOULD STILL BE EMPTY
            CryptoCurrentPrice = 20000; //GOING BACK AGAIN
            await NotificationManager.Update();
            Assert.AreEqual(true, notifications[0].Threshold == 20000); //SHOULD BE BACK
            await NotificationManager.Update();
            Assert.AreEqual(false, notifications.Any()); //AND... GONE AGAIN


            ///MOCKED VALUES - SCENARIO 2
            NotificationManager.Clear();

            //SHOW ONCE TEST
            CryptoCurrentPrice = 20001;
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BINANCE, 20000, Comparison.GREATER_THAN));

            await NotificationManager.Update();
            Assert.AreEqual(true, notifications[0].Threshold == 20000); //SHOWN 
            await NotificationManager.Update();
            Assert.AreEqual(false, notifications.Any()); //NO NOTIFICATIONS (WAS ALREADY SHOWN)

            //BREAKING THE CONDITION
            CryptoCurrentPrice = 20000;
            await NotificationManager.Update();
            Assert.AreEqual(false, notifications.Any()); //SHOULD STILL BE EMPTY
            CryptoCurrentPrice = 20001; //GOING BACK AGAIN
            await NotificationManager.Update();
            Assert.AreEqual(true, notifications[0].Threshold == 20000); //SHOULD BE BACK
            await NotificationManager.Update();
            Assert.AreEqual(false, notifications.Any()); //AND... GONE AGAIN
        }
    }
}
