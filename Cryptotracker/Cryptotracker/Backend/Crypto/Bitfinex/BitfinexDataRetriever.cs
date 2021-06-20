using Bitfinex.Net;
using Bitfinex.Net.Objects;
using Cryptotracker.Backend.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.Crypto.Bitfinex
{
    public class BitfinexDataRetriever
    {
        readonly BitfinexClient _client;
        public BitfinexDataRetriever(BitfinexClientOptions clientOptions)
        {
            _client = new BitfinexClient(clientOptions);
        }

        public async Task<CurrencyDataModel> GetCurrencyData(CryptocurrencyCode cryptocurrencyCode, CryptoInterval interval = CryptoInterval.ONE_DAY, DateTime? startTime = null, DateTime? endTime = null)
        {
            TimeFrame klineInterval = TimeFrame.OneDay;
            CryptoIntervalsDict.UniToBitfinex.TryGetValue(interval, out klineInterval);

            string symbol = $"t{cryptocurrencyCode}USD";

            var data = new CurrencyDataModel();

            bool sameDates = !startTime.HasValue || !endTime.HasValue || startTime.Value == endTime.Value;
            bool bothDatesProvided = startTime.HasValue && endTime.HasValue && !sameDates;
            bool startDateProvidedOnly = startTime.HasValue && !bothDatesProvided;
            bool endDateProvidedOnly = endTime.HasValue && !bothDatesProvided;

            if (bothDatesProvided)
            {
                //RETURNS DATA FROM GIVEN DATE RANGE

                var resultTask = await _client.GetKlinesAsync(klineInterval, symbol, null, null, startTime, endTime, Sorting.OldFirst);
                var result = resultTask.Data;

                foreach (var rate in result)
                {
                    data.Code = cryptocurrencyCode.ToString();
                    data.Rates.Add(new RateModel() { Date = rate.Timestamp, Value = (double)((rate.Low + rate.High) / 2), Low = (double)rate.Low, High = (double)rate.High });
                }
            }
            else if (startDateProvidedOnly)
            {
                //RETURNS DATA FROM START TIME TO TODAY

                var resultTask = await _client.GetKlinesAsync(klineInterval, symbol, null, null, startTime, null, Sorting.OldFirst);
                var result = resultTask.Data;

                foreach (var rate in result)
                {
                    data.Code = cryptocurrencyCode.ToString();
                    data.Rates.Add(new RateModel() { Date = rate.Timestamp, Value = (double)((rate.Low + rate.High) / 2), Low = (double)rate.Low, High = (double)rate.High });
                }
            }
            else if (endDateProvidedOnly)
            {
                return null;
            }
            else
            {
                var resultTask = await _client.GetLastKlineAsync(klineInterval, symbol);
                var result = resultTask.Data;

                data.Code = cryptocurrencyCode.ToString();
                data.Rates.Add(new RateModel() { Date = result.Timestamp, Value = (double)((result.Low + result.High) / 2), Low = (double)result.Low, High = (double)result.High });
            }

            return data;
        }
        public async Task<double> GetCurrentPrice(CryptocurrencyCode cryptocurrencyCode)
        {
            string symbol = $"t{cryptocurrencyCode}USD";
            var result = await _client.GetTickerAsync(default, symbol);
            
            return (double)result.Data.First().LastPrice;
        }
    }
}
