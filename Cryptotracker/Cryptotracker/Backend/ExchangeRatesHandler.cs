using Cryptotracker.Backend.NBP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Cryptotracker.Backend.Generic;
using Cryptotracker.Backend.Rates;

namespace Cryptotracker.Backend
{
    public static class ExchangeRatesHandler
    {
        private static HttpClient client = new HttpClient();

        private static string basicNBPAPIAddress = "http://api.nbp.pl/api/exchangerates";
        private static string basicRatesAPIAddress = "https://api.ratesapi.io/api";

        private static JsonSerializerOptions jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public static async Task<GenericCurrencyData> GetCurrencyData(ExchangePlatform currencyPlatform, CurrencyCode currencyCode, DateTime? startTime = null, DateTime? endTime = null)
        {
            string requestURI = string.Empty;
            string result = string.Empty;

            switch (currencyPlatform)
            {
                case ExchangePlatform.NBP:
                {
                    bool secondTable = false;
                    char table = 'a';
                    string currencyCodeStr = currencyCode.ToString().ToLower();

                    while (true)
                    {
                        requestURI = $"{basicNBPAPIAddress}/rates/{table}/{currencyCodeStr}";

                        if (startTime.HasValue && endTime.HasValue)
                        {
                            string startTimeStr = startTime.Value.ToString("yyyy-MM-dd");
                            requestURI += $"/{startTimeStr}";
                            string endTimeStr = endTime.Value.ToString("yyyy-MM-dd");
                            requestURI += $"/{endTimeStr}";
                        }
                        else if (startTime.HasValue)
                        {
                            string startTimeStr = startTime.Value.ToString("yyyy-MM-dd");
                            requestURI += $"/{startTimeStr}";
                        }
                        else if (endTime.HasValue && !startTime.HasValue)
                        {
                            return null;
                        }

                        try
                        {
                            result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                            break;
                        }
                        catch (Exception e)
                        {
                            if (table == 'b') //second try?
                                return null;

                            table = 'b';
                        }
                    }

                    var nbpData = JsonSerializer.Deserialize<NBPCurrencyData>(result, jsonOptions);

                    var rates = new List<GenericRate>();
                    nbpData.Rates.ForEach(x => rates.Add(new GenericRate() { Date = x.EffectiveDate, Value = x.Mid }));

                    var currencyData = new GenericCurrencyData()
                    {
                        Code = nbpData.Code,
                        Rates = rates
                    };

                    return currencyData;
                }

                case ExchangePlatform.RATES:
                {
                    string currencyCodeStr = currencyCode.ToString().ToUpper();
                    string baseAndSymbols = $"?base={currencyCodeStr}&symbols=PLN";

                    var rates = new List<GenericRate>();
                    var ratesAPIData = new RatesAPICurrencyData();

                    bool bothDatesProvided = startTime.HasValue && endTime.HasValue;
                    bool startDateProvided = startTime.HasValue;
                    bool noDateProvided = !startDateProvided && !bothDatesProvided;

                    if (bothDatesProvided)
                    {
                        var daysSpanCount = (endTime - startTime).Value.TotalDays + 1;
                        if(daysSpanCount > 0)
                        {
                            for (int i = 0; i < Math.Abs(daysSpanCount); i++)
                            {
                                requestURI = $"{basicRatesAPIAddress}/{startTime.Value.AddDays(i).ToString("yyyy-MM-dd")}{baseAndSymbols}";

                                result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                                var data = JsonSerializer.Deserialize<RatesAPICurrencyData>(result, jsonOptions);

                                double value = 0;
                                data.Rates.TryGetValue("PLN", out value);

                                rates.Add(new GenericRate() { Date = data.Date, Value = value });
                            }
                        }
                    }
                    else if (startDateProvided)
                    {
                        string startTimeStr = startTime.Value.ToString("yyyy-MM-dd");
                        requestURI = $"{basicRatesAPIAddress}/{startTimeStr}{baseAndSymbols}";

                        result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                        ratesAPIData = JsonSerializer.Deserialize<RatesAPICurrencyData>(result, jsonOptions);

                        double value = 0;
                        ratesAPIData.Rates.TryGetValue("PLN", out value);

                        rates.Add(new GenericRate() { Date = ratesAPIData.Date, Value = value });
                    }
                    else if(noDateProvided)
                    {
                        requestURI = $"{basicRatesAPIAddress}/latest{baseAndSymbols}";

                        result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                        ratesAPIData = JsonSerializer.Deserialize<RatesAPICurrencyData>(result, jsonOptions);

                        double value = 0;
                        ratesAPIData.Rates.TryGetValue("PLN", out value);

                        rates.Add(new GenericRate() { Date = ratesAPIData.Date, Value = value });
                    }

                    var currencyData = new GenericCurrencyData()
                    {
                        Code = currencyCode.ToString(),
                        Rates = rates
                    };

                    return currencyData;
                }
                break;

                default:
                    return null;
            }
        }
    }
}
