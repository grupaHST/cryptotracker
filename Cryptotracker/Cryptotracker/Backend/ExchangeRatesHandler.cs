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
using Cryptotracker.Backend.ExchangeRateHost;
using Binance.Net;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using Binance.Net.Enums;
using Bitfinex.Net;

namespace Cryptotracker.Backend
{
    public static class ExchangeRatesHandler
    {
        private static HttpClient client = new HttpClient();

        private static string basicNBPAPIAddress = "http://api.nbp.pl/api/exchangerates";
        private static string basicRatesAPIAddress = "https://api.ratesapi.io/api";
        private static string basicExchangeRateHostAddress = "https://api.exchangerate.host";

        private static JsonSerializerOptions jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        private static CurrencyCode symbol = CurrencyCode.PLN;


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
                    string baseAndSymbols = $"?base={currencyCodeStr}&symbols={symbol}";
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

                case ExchangePlatform.EXCHANGERATE_HOST:
                { 
                    string currencyCodeStr = currencyCode.ToString().ToUpper();
                    string startTimeStr = string.Empty;
                    string endTimeStr = string.Empty;

                    if (startTime.HasValue)
                        startTimeStr = startTime.Value.ToString("yyyy-MM-dd");

                    if (endTime.HasValue)
                        endTimeStr = endTime.Value.ToString("yyyy-MM-dd");

                    bool sameDates = startTime.HasValue && endTime.HasValue ? startTime.Value == endTime.Value : true;

                    var data = new ExchangeRateHostData();
                    var rates = new List<GenericRate>();

                    bool bothDatesProvided = startTime.HasValue && endTime.HasValue && !sameDates;
                    bool startDateProvided = !bothDatesProvided && startTime.HasValue;

                    if (bothDatesProvided)
                    {
                        requestURI = $"{basicExchangeRateHostAddress}/timeseries?base={currencyCodeStr}&symbols={symbol}&start_date={startTimeStr}&end_date={endTimeStr}";

                        try
                        {
                            result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                            data = JsonSerializer.Deserialize<ExchangeRateHostData>(result, jsonOptions);
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

                            rates.Add(new GenericRate() { Date = rate.Key, Value = value });
                        }
                    }
                    else if (startDateProvided)
                    {
                        requestURI = $"{basicExchangeRateHostAddress}/timeseries?base={currencyCodeStr}&symbols={symbol}&start_date={startTimeStr}&end_date={startTimeStr}";

                        try
                        {
                            result = await client.GetStringAsync(requestURI).ConfigureAwait(false);
                            data = JsonSerializer.Deserialize<ExchangeRateHostData>(result, jsonOptions);
                        }
                        catch (Exception e)
                        {
                            //(Application.Current as App).LogMessage($"NBP: No data for given day. {e.Message}");
                            return null;
                        }

                        double value = 0;
                        data.Rates.First().Value.TryGetValue("PLN", out value);

                        rates.Add(new GenericRate() { Date = data.Rates.First().Key, Value = value });
                    }
                    else if (endTime.HasValue && !startTime.HasValue)
                    {
                        //(Application.Current as App).LogMessage("NBP: No start date has been provided.");
                        return null;
                    }
                    else
                    {
                        requestURI = $"{basicExchangeRateHostAddress}/latest?base={currencyCodeStr}&symbols={symbol}";

                        try
                        {
                            result = await client.GetStringAsync(requestURI);
                            data = JsonSerializer.Deserialize<ExchangeRateHostData>(result, jsonOptions);

                        }
                        catch (Exception)
                        {
                            return null;
                        }

                        double value = 0;
                        data.Rates.First().Value.TryGetValue("PLN", out value);

                        rates.Add(new GenericRate() { Date = data.Rates.First().Key, Value = value });
                    }

                    if (!data.Rates.Any())
                    {
                        return null;
                    }

                    return new GenericCurrencyData()
                    {
                        Code = data.Base,
                        Rates = rates
                    };

                }
                break;

                default:
                    return null;
            }
        }
        /// <summary>
        /// Method meant to return cryptocurrency data. If interval is provided then startTime is also required. If no startTime is provided then interval is ignored.
        /// </summary>
        /// <param name="cryptoPlatform">Crypto platform choice</param>
        /// <param name="cryptocurrencyCode">Currency choice (value returned in BUSD by default)</param>
        /// <param name="interval">Interval between rates. At least one time is required for this to be used</param>
        /// <param name="startTime">Start date of data range</param>
        /// <param name="endTime">End date of data range</param>
        /// <returns></returns>
        public static async Task<GenericCurrencyData> GetCryptocurrencyData(CryptoExchangePlatform cryptoPlatform, CryptocurrencyCode cryptocurrencyCode, CryptoInterval interval = CryptoInterval.ONE_DAY, DateTime ? startTime = null, DateTime? endTime = null)
        {
            switch(cryptoPlatform)
            {
                case CryptoExchangePlatform.BINANCE:
                {
                    return await GetBinanceCurrencyData(cryptocurrencyCode, interval, startTime, endTime);
                }
                break;

                default:
                return null;
            }
        }

        private static async Task<GenericCurrencyData> GetBinanceCurrencyData(CryptocurrencyCode cryptocurrencyCode, CryptoInterval interval = CryptoInterval.ONE_DAY, DateTime ? startTime = null, DateTime? endTime = null)
        {
            BinanceClient client = new BinanceClient(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials("ZD4viGXGIAdLEOUN5vOIF4LQfcBQhrcdbX8r9oUd1ACo29kpd35G0g6uXl7nLRnh", "YdR3dMvkcEtGboOm4SnfOwnYRR6qhmTFCPfPcXO0yerYi1LCO2QhCmP5tUj9c8He"),
                AutoTimestamp = true,
                AutoTimestampRecalculationInterval = TimeSpan.FromMinutes(30)
            });

            KlineInterval klineInterval = KlineInterval.OneDay;
            CryptoIntervalsDict.UniToBinance.TryGetValue(interval, out klineInterval);

            string symbol = $"{cryptocurrencyCode}BUSD";

            var data = new GenericCurrencyData();

            bool sameDates = startTime.HasValue && endTime.HasValue ? startTime.Value == endTime.Value : true;
            bool bothDatesProvided = startTime.HasValue && endTime.HasValue && !sameDates;
            bool startDateProvidedOnly = startTime.HasValue && !bothDatesProvided;
            bool endDateProvidedOnly = endTime.HasValue && !bothDatesProvided;

            if(bothDatesProvided)
            {
                //RETURNS DATA FROM GIVEN DATE RANGE
                var result = client.Spot.Market.GetKlines(symbol, klineInterval, startTime, endTime).Data;

                foreach (var rate in result)
                {
                    data.Code = cryptocurrencyCode.ToString();
                    data.Rates.Add(new GenericRate() { Date = rate.OpenTime, Value = (double)((rate.Low + rate.High) / 2), Low = (double)rate.Low, High = (double)rate.High });
                }
            }
            else if(startDateProvidedOnly)
            {
                //RETURNS DATA FROM START TIME TO TODAY

                var result = client.Spot.Market.GetKlines(symbol, klineInterval, startTime).Data;

                foreach (var rate in result)
                {
                    data.Code = cryptocurrencyCode.ToString();
                    data.Rates.Add(new GenericRate() { Date = rate.OpenTime, Value = (double)((rate.Low + rate.High) / 2), Low = (double)rate.Low, High = (double)rate.High });
                }
            }
            else if(endDateProvidedOnly)
            {
                return null;
            }
            else
            {
                var result = client.Spot.Market.Get24HPrice(symbol).Data;

                data.Code = cryptocurrencyCode.ToString();
                data.Rates.Add(new GenericRate() { Date = result.OpenTime, Value = (double)((result.LowPrice + result.HighPrice) / 2), Low = (double)result.LowPrice, High = (double)result.HighPrice });
            }

            return data;
        }
    }
}
