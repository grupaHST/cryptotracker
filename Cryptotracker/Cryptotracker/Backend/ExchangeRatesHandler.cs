using Cryptotracker.Backend.NBP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Cryptotracker.Backend.Generic;

namespace Cryptotracker.Backend
{
    public static class ExchangeRatesHandler
    {
        private static HttpClient client = new HttpClient();
        private static string basicNBPApiAddress = "http://api.nbp.pl/api/exchangerates";

        public static async Task<GenericCurrencyData> GetCurrencyData(ExchangePlatform currencyPlatform, CurrencyCode currencyCode, DateTime? startTime = null, DateTime? endTime = null)
        {
            string requestURI = string.Empty;
            string currencyCodeStr = currencyCode.ToString().ToLower();
            string result = string.Empty;

            char table = 'a';

            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.PropertyNameCaseInsensitive = true;

            switch (currencyPlatform)
            {
                case ExchangePlatform.NBP:
                {
                    requestURI = $"{basicNBPApiAddress}/rates/{table}/{currencyCodeStr}";

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
                        return new GenericCurrencyData() { ErrorMessage = "No start date provided!", ErrorOccured = true };
                    }

                    bool secondTable = false;
                    while (true)
                    {
                        try
                        {
                            if(secondTable)
                            {
                                table = 'b';
                                requestURI = $"{basicNBPApiAddress}/rates/{table}/{currencyCodeStr}";

                                result = await client.GetStringAsync(requestURI).ConfigureAwait(false);

                                break;
                            }

                            result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                            
                            break;
                        }
                        catch (Exception e)
                        {
                            if (secondTable)
                                return new GenericCurrencyData() { ErrorMessage = e.Message, ErrorOccured = true };

                            secondTable = true;
                        }
                    }

                    var nbpData = JsonSerializer.Deserialize<NBPCurrencyData>(result, jsonOptions);

                    var rates = new List<GenericRate>();
                    nbpData.Rates.ForEach(x => rates.Add(new GenericRate() { Date = x.EffectiveDate, Value = x.Mid }));

                    var currencyData = new GenericCurrencyData()
                    {
                        Code = nbpData.Code,
                        CurrencyName = nbpData.Currency,
                        Rates = rates
                    };

                    return currencyData;
                }

                default:
                    return new GenericCurrencyData() { ErrorMessage = "Invalid platform!", ErrorOccured = true };
            }
        }
    }
}
