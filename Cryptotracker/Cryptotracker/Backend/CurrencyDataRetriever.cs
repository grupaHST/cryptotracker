using System;
using System.Threading.Tasks;

namespace Cryptotracker.Backend
{
    public abstract class CurrencyDataRetriever
    {
        public abstract Task<CurrencyDataModel> GetCurrencyData(ExchangePlatform currencyPlatform, CurrencyCode currencyCode, DateTime? startTime = null, DateTime? endTime = null);
    }
}
