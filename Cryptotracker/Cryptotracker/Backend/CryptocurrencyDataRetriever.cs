using System;
using System.Threading.Tasks;

namespace Cryptotracker.Backend
{
    public abstract class CryptocurrencyDataRetriever
    {
        public abstract Task<CurrencyDataModel> GetCurrencyData(CryptocurrencyCode cryptocurrencyCode, CryptoInterval interval = CryptoInterval.ONE_DAY, DateTime? startTime = null, DateTime? endTime = null);
        public abstract Task<double> GetCurrentPrice(CryptocurrencyCode cryptocurrencyCode);
    }
}
