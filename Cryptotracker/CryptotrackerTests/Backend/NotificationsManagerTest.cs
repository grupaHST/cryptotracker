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
        public void OnNotificationOccurenceTest(object? sender, List<NotificationModel> notificationsToDisplay)
        {
            //BINANCE
            Assert.AreEqual(true, notificationsToDisplay[0].Threshold == 20000);
            Assert.AreEqual(true, notificationsToDisplay[0].CurrentValue > 20000);
            Assert.AreEqual(true, notificationsToDisplay[1].Threshold == 1500);
            Assert.AreEqual(false, (notificationsToDisplay[2].Threshold == 1000000 && notificationsToDisplay[2].CryptocurrencyCode == CryptocurrencyCode.BTC));
            Assert.AreEqual(true, notificationsToDisplay[2].Threshold == 1000000);
            Assert.AreEqual(true, notificationsToDisplay[2].CurrentValue < 1000000);

            //BITFINEX
            Assert.AreEqual(true, notificationsToDisplay[3].CurrentValue > 20000);
            Assert.AreEqual(true, notificationsToDisplay[3].Threshold == 20000);
            Assert.AreEqual(true, notificationsToDisplay[4].CurrentValue > 1000);
            Assert.AreEqual(true, notificationsToDisplay[4].Threshold == 1000);
            Assert.AreEqual(true, notificationsToDisplay[5].CurrentValue > 10000);
            Assert.AreEqual(true, notificationsToDisplay[5].Threshold == 10000);

            //NBP
            Assert.AreEqual(true, notificationsToDisplay[6].CurrentValue > 2);
            Assert.AreEqual(true, notificationsToDisplay[6].Threshold == 2);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            // TESTING API KEYS
            ExchangeRatesHandler.BitfinexAPIKey = "mDqhNh8HLJmbuBdCSLktNXhpsaqtA4FSdAOutllUoWh";
            ExchangeRatesHandler.BitfinexAPISecret = "jv3bsJhv6L0lUYPHbO1KXsPPfzcP5rrcfRjt80QO0t8";
            ExchangeRatesHandler.BinanceAPIKey = "ZD4viGXGIAdLEOUN5vOIF4LQfcBQhrcdbX8r9oUd1ACo29kpd35G0g6uXl7nLRnh";
            ExchangeRatesHandler.BinanceAPISecret = "YdR3dMvkcEtGboOm4SnfOwnYRR6qhmTFCPfPcXO0yerYi1LCO2QhCmP5tUj9c8He";

            NotificationManager.EventHandler += OnNotificationOccurenceTest;

            //BINANCE
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BINANCE, 20000, Comparison.GREATER_THAN));
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.ETH, CryptoExchangePlatform.BINANCE, 1500, Comparison.GREATER_THAN_OR_EQUAL));
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BINANCE, 1000000, Comparison.GREATER_THAN)); //FALSE
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.ETH, CryptoExchangePlatform.BINANCE, 1000000, Comparison.LESS_THAN_OR_EQUAL));

            //BITFINEX
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BITFINEX, 20000, Comparison.GREATER_THAN));
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.ETH, CryptoExchangePlatform.BITFINEX, 1000, Comparison.GREATER_THAN));
            NotificationManager.AddNotification(new NotificationModel(CryptocurrencyCode.BTC, CryptoExchangePlatform.BITFINEX, 10000, Comparison.GREATER_THAN));

            //NBP
            NotificationManager.AddNotification(new NotificationModel(CurrencyCode.USD, ExchangePlatform.NBP, 2, Comparison.GREATER_THAN));
            NotificationManager.AddNotification(new NotificationModel(CurrencyCode.USD, ExchangePlatform.NBP, 2, Comparison.LESS_THAN)); //FALSE

            await NotificationManager.Update();
        }
    }
}
