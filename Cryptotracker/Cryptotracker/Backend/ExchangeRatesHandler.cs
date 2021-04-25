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
using System.Windows;

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
                    string currencyCodeStr = currencyCode.ToString().ToLower();
                    string startTimeStr = string.Empty;
                    string endTimeStr = string.Empty;

                    //check if currency is available in table a
                    char table = 'a';

                    try
                    {
                        requestURI = $"{basicNBPAPIAddress}/rates/{table}/{currencyCodeStr}";
                        var test = await client.GetStringAsync(requestURI);
                    }
                    catch (Exception e)
                    {
                        table = 'b';
                    }

                    if (startTime.HasValue)
                        startTimeStr = startTime.Value.ToString("yyyy-MM-dd");

                    if (endTime.HasValue)
                        endTimeStr = endTime.Value.ToString("yyyy-MM-dd");

                    bool sameDates = startTime.HasValue && endTime.HasValue ? startTime.Value == endTime.Value : true;

                    var nbpData = new NBPCurrencyData();
                    var rates = new List<GenericRate>();

                    if (startTime.HasValue && endTime.HasValue && !sameDates)
                    {
                        requestURI = $"{basicNBPAPIAddress}/rates/{table}/{currencyCodeStr}/{startTimeStr}/{endTimeStr}";

                        try
                        {
                            result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                            nbpData = JsonSerializer.Deserialize<NBPCurrencyData>(result, jsonOptions);
                        }
                        catch (Exception e)
                        {
                            //(Application.Current as App).LogMessage($"NBP: No data for given days. {e.Message}");
                            return null;
                        }

                        foreach (var rate in nbpData.Rates)
                        {
                            rates.Add(new GenericRate() { Date = rate.EffectiveDate, Value = rate.Mid });
                        }
                    }
                    else if (startTime.HasValue)
                    {
                        requestURI = $"{basicNBPAPIAddress}/rates/{table}/{currencyCodeStr}/{startTimeStr}";

                        try
                        {
                            result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                            nbpData = JsonSerializer.Deserialize<NBPCurrencyData>(result, jsonOptions);
                        }
                        catch (Exception e)
                        {
                            //(Application.Current as App).LogMessage($"NBP: No data for given day. {e.Message}");
                            return null;
                        }

                        rates.Add(new GenericRate() { Date = nbpData.Rates.First().EffectiveDate, Value = nbpData.Rates.First().Mid });
                    }
                    else if (endTime.HasValue && !startTime.HasValue)
                    {
                        //(Application.Current as App).LogMessage("NBP: No start date has been provided.");
                        return null;
                    }
                    else
                    {
                        requestURI = $"{basicNBPAPIAddress}/rates/{table}/{currencyCodeStr}";

                        try
                        {
                            result = await client.GetStringAsync(requestURI);
                            nbpData = JsonSerializer.Deserialize<NBPCurrencyData>(result, jsonOptions);

                        }
                        catch (Exception)
                        {
                            return null;
                        }

                        rates.Add(new GenericRate() { Date = nbpData.Rates.First().EffectiveDate, Value = nbpData.Rates.First().Mid });
                    }

                    return new GenericCurrencyData()
                    {
                        Code = nbpData.Code,
                        Rates = rates
                    };
                }

                case ExchangePlatform.RATES:
                {
                    string currencyCodeStr = currencyCode.ToString().ToUpper();
                    string baseAndSymbols = $"?base={currencyCodeStr}&symbols=PLN";
                    string startTimeStr = string.Empty;
                    string endTimeStr = string.Empty;

                    if (startTime.HasValue)
                        startTimeStr = startTime.Value.ToString("yyyy-MM-dd");

                    if (endTime.HasValue)
                        endTimeStr = endTime.Value.ToString("yyyy-MM-dd");


                    var rates = new List<GenericRate>();
                    var ratesAPIData = new RatesAPICurrencyData();

                    bool sameDates = startTime.HasValue && endTime.HasValue ? startTime.Value == endTime.Value : true;

                    bool bothDatesProvided = startTime.HasValue && endTime.HasValue && !sameDates;
                    bool startDateProvided = startTime.HasValue;
                    bool noDateProvided = !startDateProvided && !bothDatesProvided;

                    if (bothDatesProvided)
                    {
                        var daysSpanCount = (endTime - startTime).Value.TotalDays + 1;
                        if(daysSpanCount > 0)
                        {
                            for (int i = 0; i < Math.Abs(daysSpanCount); i++)
                            {
                                var startTimeOffset = startTime.Value.AddDays(i);
                                requestURI = $"{basicRatesAPIAddress}/{startTimeOffset.ToString("yyyy-MM-dd")}{baseAndSymbols}";

                                try
                                {
                                    result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                                }
                                catch (Exception e)
                                {
                                    //(Application.Current as App).LogMessage($"RatesAPI: Data range unavailable. Fatal Error. {e.Message}");
                                    return null;                                    
                                }

                                ratesAPIData = JsonSerializer.Deserialize<RatesAPICurrencyData>(result, jsonOptions);

                                double value = 0;
                                ratesAPIData.Rates.TryGetValue("PLN", out value);

                                bool realDataReceived = ratesAPIData.Date == startTimeOffset;
                                if (realDataReceived)
                                {
                                    rates.Add(new GenericRate() { Date = ratesAPIData.Date, Value = value });
                                }
                                else
                                {
                                    //(Application.Current as App).LogMessage($"RatesAPI: Data for {startTimeOffset} unavailable.");
                                }
                            }
                        }
                    }
                    else if (startDateProvided)
                    {
                        requestURI = $"{basicRatesAPIAddress}/{startTimeStr}{baseAndSymbols}";

                        try
                        {
                            result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                            if (result == null)
                            {
                                return null;
                            }
                        }
                        catch (Exception e)
                        {
                            //(Application.Current as App).LogMessage($"RatesAPI: Data for given day is unavailable. {e.Message}");
                            return null;
                        }

                        ratesAPIData = JsonSerializer.Deserialize<RatesAPICurrencyData>(result, jsonOptions);

                        double value = 0;
                        ratesAPIData.Rates.TryGetValue("PLN", out value);

                        bool realDataReceived = ratesAPIData.Date == startTime.Value;
                        if (realDataReceived)
                        {
                            rates.Add(new GenericRate() { Date = ratesAPIData.Date, Value = value });
                        }
                        else
                        {
                            //(Application.Current as App).LogMessage($"RatesAPI: Data for {startTime.Value} unavailable.");
                            return null;
                        }
                    }
                    else if(noDateProvided)
                    {
                        requestURI = $"{basicRatesAPIAddress}/latest{baseAndSymbols}";

                        try
                        {
                            result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            //(Application.Current as App).LogMessage($"RatesAPI: Latest data unavailable! {e.Message}");
                            return null;
                        }

                        ratesAPIData = JsonSerializer.Deserialize<RatesAPICurrencyData>(result, jsonOptions);

                        double value = 0;
                        ratesAPIData.Rates.TryGetValue("PLN", out value);

                        rates.Add(new GenericRate() { Date = ratesAPIData.Date, Value = value });
                    }

                    return new GenericCurrencyData()
                    {
                        Code = currencyCode.ToString(),
                        Rates = rates
                    };
                }
                break;

                default:
                    return null;
            }
        }
    }
}
