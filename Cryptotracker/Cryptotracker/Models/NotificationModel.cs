using Cryptotracker.Backend;
using Cryptotracker.Backend.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Models
{
    public class NotificationModel
    {
        public enum CurrencyTypeEnum
        {
            CRYPTO,
            FIAT
        }

        public readonly CurrencyCode CurrencyCode;
        public readonly CryptocurrencyCode CryptocurrencyCode;
        public readonly ExchangePlatform ExchangePlatform;
        public readonly CryptoExchangePlatform CryptoExchangePlatform;
        public readonly double Threshold;
        public readonly Comparison Comparison;
        public readonly CurrencyTypeEnum CurrencyType;
        public double CurrentValue { get; set; }

        public bool WasDisplayed { get; set; }

        public NotificationModel(CurrencyCode currencyCode, ExchangePlatform exchangePlatform, double threshold, Comparison comparison)
        {
            CurrencyCode = currencyCode;
            ExchangePlatform = exchangePlatform;
            Threshold = threshold;
            Comparison = comparison;
            CurrencyType = CurrencyTypeEnum.FIAT;
        }

        public NotificationModel(CryptocurrencyCode cryptocurrencyCode, CryptoExchangePlatform cryptoExchangePlatform, double threshold, Comparison comparison)
        {
            CryptocurrencyCode = cryptocurrencyCode;
            CryptoExchangePlatform = cryptoExchangePlatform;
            Threshold = threshold;
            Comparison = comparison;
            CurrencyType = CurrencyTypeEnum.CRYPTO;
        }
    }
}
