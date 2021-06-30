using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend
{
    public static class CurrencyExchangeCalculator
    {
        public static async Task<double?> GetCurrentExchangeValue(string currencyCode, string exchangePlatform)
        {
            if (currencyCode == "PLN")
                return 1;
            else
                return await ExchangeRatesHandler.GetFIATCurrentPrice(Enum.Parse<ExchangePlatform>(exchangePlatform), Enum.Parse<CurrencyCode>(currencyCode));
        }

        public static decimal CalculateValue(decimal firstCurrencyValue, double? firstCurrencyExchangeValue, double? secondCurrencyExchangeValue)
        {
            if (firstCurrencyExchangeValue is not null && secondCurrencyExchangeValue is not null)
            {
                var ratio = (double)firstCurrencyExchangeValue / (double)secondCurrencyExchangeValue;
                return firstCurrencyValue * (decimal)ratio;
            }
            return 0;

        }
    }
}
