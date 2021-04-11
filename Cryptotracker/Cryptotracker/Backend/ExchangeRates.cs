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
    public static class ExchangeRates
    {
        private static HttpClient client = new HttpClient();
        private static string basicNBPApiAddress = "http://api.nbp.pl/api/exchangerates";

        public static async Task<GenericCurrencyData> GetCurrencyData(ExchangePlatform currencyPlatform, CurrencyCode currencyCode)
        {
            string requestURI = string.Empty;
            string currencyCodeStr = currencyCode.ToString().ToLower();
            
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.PropertyNameCaseInsensitive = true;

            switch (currencyPlatform)
            {
                case ExchangePlatform.NBP:
                {
                    try
                    {
                        requestURI = $"{basicNBPApiAddress}/rates/a/{currencyCodeStr}/";
                        var result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
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
                    catch (Exception e)
                    {
                        return new GenericCurrencyData() { ErrorMessage = e.Message, ErrorOccured = true };
                    }
                }

                default:
                    return new GenericCurrencyData() { ErrorMessage = "Invalid platform", ErrorOccured = true };
            }
        }
    }
}
