using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects.Spot;
using Cryptotracker.Backend.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.Crypto.Binance
{
    public class BinanceDataRetriever
    {
        readonly BinanceClient _client;
        public BinanceDataRetriever(BinanceClientOptions clientOptions)
        {
            _client = new BinanceClient(clientOptions);
        }
        public async Task<CurrencyDataModel> GetCurrencyData(CryptocurrencyCode cryptocurrencyCode, CryptoInterval interval = CryptoInterval.ONE_DAY, DateTime? startTime = null, DateTime? endTime = null)
        {
            KlineInterval klineInterval = KlineInterval.OneDay;
            CryptoIntervalsDict.UniToBinance.TryGetValue(interval, out klineInterval);

            string symbol = $"{cryptocurrencyCode}BUSD";

            var data = new CurrencyDataModel();

            bool sameDates = !startTime.HasValue || !endTime.HasValue || startTime.Value.ToString() == endTime.Value.ToString();
            bool bothDatesProvided = startTime.HasValue && endTime.HasValue && !sameDates;
            bool startDateProvidedOnly = startTime.HasValue && !bothDatesProvided;
            bool endDateProvidedOnly = endTime.HasValue && !bothDatesProvided;

            if (bothDatesProvided)
            {
                //RETURNS DATA FROM GIVEN DATE RANGE
                var resultTask = await _client.Spot.Market.GetKlinesAsync(symbol, klineInterval, startTime, endTime);
                var result = resultTask.Data;

                foreach (var rate in result)
                {
                    data.Code = cryptocurrencyCode.ToString();
                    data.Rates.Add(new RateModel() { Date = rate.OpenTime, Value = (double)((rate.Low + rate.High) / 2), Low = (double)rate.Low, High = (double)rate.High });
                }
            }
            else if (startDateProvidedOnly)
            {
                //RETURNS DATA FROM START TIME TO TODAY

                var resultTask = await _client.Spot.Market.GetKlinesAsync(symbol, klineInterval, startTime);
                var result = resultTask.Data;

                foreach (var rate in result)
                {
                    data.Code = cryptocurrencyCode.ToString();
                    data.Rates.Add(new RateModel() { Date = rate.OpenTime, Value = (double)((rate.Low + rate.High) / 2), Low = (double)rate.Low, High = (double)rate.High });
                }
            }
            else if (endDateProvidedOnly)
            {
                return null;
            }
            else
            {
                var resultTask = await _client.Spot.Market.Get24HPriceAsync(symbol);
                var result = resultTask.Data;

                data.Code = cryptocurrencyCode.ToString();
                data.Rates.Add(new RateModel() { Date = result.OpenTime, Value = (double)((result.LowPrice + result.HighPrice) / 2), Low = (double)result.LowPrice, High = (double)result.HighPrice });
            }

            return data;
        }

        public async Task<double> GetCurrentPrice(CryptocurrencyCode cryptocurrencyCode)
        {
            string symbol = $"{cryptocurrencyCode}BUSD";

            var result = await _client.Spot.Market.GetCurrentAvgPriceAsync(symbol);

            return (double)result.Data.Price;
        }
    }
}
