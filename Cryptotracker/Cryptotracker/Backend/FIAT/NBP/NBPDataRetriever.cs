using Cryptotracker.Backend.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.FIAT.NBP
{
    public class NBPDataRetriever : CurrencyDataRetriever
    {
        readonly string _basicAPIAddress = "http://api.nbp.pl/api/exchangerates";
        readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };
        readonly HttpClient _client = new HttpClient();

        public override async Task<CurrencyDataModel> GetCurrencyData(ExchangePlatform currencyPlatform, CurrencyCode currencyCode, DateTime? startTime = null, DateTime? endTime = null)
        {
            string requestURI = string.Empty;
            string result = string.Empty;

            string currencyCodeStr = currencyCode.ToString().ToLower();
            string startTimeStr = string.Empty;
            string endTimeStr = string.Empty;

            //check if currency is available in table a
            char table = 'a';

            try
            {
                requestURI = $"{_basicAPIAddress}/rates/{table}/{currencyCodeStr}";
                var test = await _client.GetStringAsync(requestURI);
            }
            catch (Exception)
            {
                table = 'b';
            }

            if (startTime.HasValue)
            {
                startTimeStr = startTime.Value.ToString("yyyy-MM-dd");
            }

            if (endTime.HasValue)
            {
                endTimeStr = endTime.Value.ToString("yyyy-MM-dd");
            }

            bool sameDates = !startTime.HasValue || !endTime.HasValue || startTime.Value == endTime.Value;

            var nbpData = new NBPCurrencyData();
            var rates = new List<RateModel>();

            if (startTime.HasValue && endTime.HasValue && !sameDates)
            {
                requestURI = $"{_basicAPIAddress}/rates/{table}/{currencyCodeStr}/{startTimeStr}/{endTimeStr}";

                try
                {
                    result = await _client.GetStringAsync(requestURI).ConfigureAwait(false);
                    nbpData = JsonSerializer.Deserialize<NBPCurrencyData>(result, jsonOptions);
                }
                catch (Exception e)
                {
                    //(Application.Current as App).LogMessage($"NBP: No data for given days. {e.Message}");
                    return null;
                }

                foreach (var rate in nbpData.Rates)
                {
                    rates.Add(new RateModel() { Date = rate.EffectiveDate, Value = rate.Mid });
                }
            }
            else if (startTime.HasValue)
            {
                requestURI = $"{_basicAPIAddress}/rates/{table}/{currencyCodeStr}/{startTimeStr}";

                try
                {
                    result = await _client.GetStringAsync(requestURI).ConfigureAwait(false);
                    nbpData = JsonSerializer.Deserialize<NBPCurrencyData>(result, jsonOptions);
                }
                catch (Exception e)
                {
                    //(Application.Current as App).LogMessage($"NBP: No data for given day. {e.Message}");
                    return null;
                }

                rates.Add(new RateModel() { Date = nbpData.Rates.First().EffectiveDate, Value = nbpData.Rates.First().Mid });
            }
            else if (endTime.HasValue && !startTime.HasValue)
            {
                //(Application.Current as App).LogMessage("NBP: No start date has been provided.");
                return null;
            }
            else
            {
                requestURI = $"{_basicAPIAddress}/rates/{table}/{currencyCodeStr}";

                try
                {
                    result = await _client.GetStringAsync(requestURI);
                    nbpData = JsonSerializer.Deserialize<NBPCurrencyData>(result, jsonOptions);

                }
                catch (Exception)
                {
                    return null;
                }

                rates.Add(new RateModel() { Date = nbpData.Rates.First().EffectiveDate, Value = nbpData.Rates.First().Mid });
            }

            return new CurrencyDataModel()
            {
                Code = nbpData.Code,
                Rates = rates
            };
        }
    }
}
