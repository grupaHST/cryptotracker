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
    }
}
