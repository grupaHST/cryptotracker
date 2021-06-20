using Cryptotracker.Backend.ExchangeRateHost;
using Cryptotracker.Backend.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.FIAT.ExchangeRateHost
{
    public class ExchangeRateHostDataRetriever : CurrencyDataRetriever
    {
        readonly string _basicAPIAddress = "https://api.exchangerate.host";

        readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        readonly HttpClient _client = new HttpClient();

        readonly CurrencyCode _symbol = CurrencyCode.PLN;

        public override async Task<CurrencyDataModel> GetCurrencyData(ExchangePlatform currencyPlatform, CurrencyCode currencyCode, DateTime? startTime = null, DateTime? endTime = null)
        {

            string currencyCodeStr = currencyCode.ToString().ToUpper();
            string startTimeStr = string.Empty;
            string endTimeStr = string.Empty;

            if (startTime.HasValue)
                startTimeStr = startTime.Value.ToString("yyyy-MM-dd");

            if (endTime.HasValue)
                endTimeStr = endTime.Value.ToString("yyyy-MM-dd");

            bool sameDates = !startTime.HasValue || !endTime.HasValue || startTime.Value == endTime.Value;

            var data = new ExchangeRateHostData();
            var rates = new List<RateModel>();

            bool bothDatesProvided = startTime.HasValue && endTime.HasValue && !sameDates;
            bool startDateProvided = !bothDatesProvided && startTime.HasValue;

            string requestURI;
            string result;

            if (bothDatesProvided)
            {
                requestURI = $"{_basicAPIAddress}/timeseries?base={currencyCodeStr}&symbols={_symbol}&start_date={startTimeStr}&end_date={endTimeStr}";

                try
                {
                    result = await _client.GetStringAsync(requestURI).ConfigureAwait(false);
                    data = JsonSerializer.Deserialize<ExchangeRateHostData>(result, _jsonOptions);
                }
                catch (Exception e)
                {
                    //(Application.Current as App).LogMessage($"NBP: No data for given days. {e.Message}");
                    return null;
                }

                foreach (var rate in data.Rates)
                {
                    double value = 0;
                    rate.Value.TryGetValue("PLN", out value);

                    rates.Add(new RateModel() { Date = rate.Key, Value = value });
                }
            }
            else if (startDateProvided)
            {
                requestURI = $"{_basicAPIAddress}/timeseries?base={currencyCodeStr}&symbols={_symbol}&start_date={startTimeStr}&end_date={startTimeStr}";

                try
                {
                    result = await _client.GetStringAsync(requestURI).ConfigureAwait(false);
                    data = JsonSerializer.Deserialize<ExchangeRateHostData>(result, _jsonOptions);
                }
                catch (Exception e)
                {
                    //(Application.Current as App).LogMessage($"NBP: No data for given day. {e.Message}");
                    return null;
                }

                double value = 0;
                data.Rates.First().Value.TryGetValue("PLN", out value);

                rates.Add(new RateModel() { Date = data.Rates.First().Key, Value = value });
            }
            else if (endTime.HasValue && !startTime.HasValue)
            {
                //(Application.Current as App).LogMessage("NBP: No start date has been provided.");
                return null;
            }
            else
            {
                requestURI = $"{_basicAPIAddress}/latest?base={currencyCodeStr}&symbols={_symbol}";

                try
                {
                    result = await _client.GetStringAsync(requestURI);
                    data = JsonSerializer.Deserialize<ExchangeRateHostData>(result, _jsonOptions);

                }
                catch (Exception)
                {
                    return null;
                }

                double value = 0;
                data.Rates.First().Value.TryGetValue("PLN", out value);

                rates.Add(new RateModel() { Date = data.Rates.First().Key, Value = value });
            }

            if (!data.Rates.Any())
            {
                return null;
            }

            return new CurrencyDataModel()
            {
                Code = data.Base,
                Rates = rates
            };
        }
    }
}
