using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Cryptotracker.Models;

namespace Cryptotracker.Backend.Notifications
{
    public enum Comparison
    {
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL,
        EQUAL,
        LESS_THAN,
        LESS_THAN_OR_EQUAL,
    }

    public static class NotificationManager
    {
        private static List<NotificationModel> notificationModels = new List<NotificationModel>();

        public static EventHandler<List<NotificationModel>> EventHandler;
        public static void Init()
        {
            DispatcherTimer updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMinutes(1);
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private static async void UpdateTimer_Tick(object sender, EventArgs e)
        {
            await Task.Run(() => Update());
        }

        public static async Task Update()
        {
            if (notificationModels.Any())
            {
                List<NotificationModel> notificationsToDisplay = new List<NotificationModel>();

                double currentPrice = 0;

                foreach (var notification in notificationModels)
                { 
                    switch(notification.CurrencyType)
                    {
                        case NotificationModel.CurrencyTypeEnum.CRYPTO:
                        {
                            currentPrice = await ExchangeRatesHandler.GetCryptoCurrentPrice(notification.CryptoExchangePlatform, notification.CryptocurrencyCode);
                        }
                        break;

                        case NotificationModel.CurrencyTypeEnum.FIAT:
                        {
                            var currencyData = await ExchangeRatesHandler.GetCurrencyData(notification.ExchangePlatform, notification.CurrencyCode, DateTime.Today, DateTime.Today);
                            currentPrice = currencyData.Rates.First().Value;
                        }
                        break;
                    }

                    if(CheckCondition(notification.Threshold, notification.Comparison, currentPrice))
                    {
                        notification.CurrentValue = currentPrice;
                        notificationsToDisplay.Add(notification);
                    }
                }

                if (notificationsToDisplay.Any())
                {
                    EventHandler.Invoke(null, notificationsToDisplay);
                }

                notificationModels.Clear(); //TODO: Temporarily used to prevent looping
            }
        }

        private static bool CheckCondition(double setvalue, Comparison comparison, double currentValue)
        {
            switch (comparison)
            {
                case Comparison.GREATER_THAN:
                    return currentValue > setvalue;
                case Comparison.GREATER_THAN_OR_EQUAL:
                    return currentValue >= setvalue;
                case Comparison.EQUAL:
                    return currentValue == setvalue;
                case Comparison.LESS_THAN:
                    return currentValue < setvalue;
                case Comparison.LESS_THAN_OR_EQUAL:
                    return currentValue <= setvalue;
                default:
                    return false;
            }
        }

        public static void AddNotification(NotificationModel notificationModel) => notificationModels.Add(notificationModel);
    }
}
