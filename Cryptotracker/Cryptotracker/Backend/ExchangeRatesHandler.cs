
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Cryptotracker.Backend.Generic;
using System.Windows;
using Cryptotracker.Backend.ExchangeRateHost;
using Binance.Net;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using Binance.Net.Enums;
using Bitfinex;
using Bitfinex.Net;
using Bitfinex.Net.Objects;
using Cryptotracker.ViewModels;
using YahooFinanceApi;
using Cryptotracker.Backend.Crypto.Bitfinex;
using Cryptotracker.Backend.Crypto.Binance;

namespace Cryptotracker.Backend
{
    public static class ExchangeRatesHandler
    {
        public static string BinanceAPIKey { get; set; }
        public static string BinanceAPISecret { get; set; }

        public static string BitfinexAPIKey { get; set; }
        public static string BitfinexAPISecret { get; set; }

        private static readonly HttpClient client = new();

        private static readonly string basicNBPAPIAddress = "http://api.nbp.pl/api/exchangerates";
        private static readonly string basicExchangeRateHostAddress = "https://api.exchangerate.host";

        private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

        private static readonly CurrencyCode symbol = CurrencyCode.PLN;

        public static async Task<CurrencyDataModel> GetCurrencyData(ExchangePlatform currencyPlatform, CurrencyCode currencyCode, DateTime? startTime = null, DateTime? endTime = null)
        {
            string requestURI;
            string result;
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
                            rates.Add(new RateModel() { Date = rate.EffectiveDate, Value = rate.Mid });
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

                        rates.Add(new RateModel() { Date = nbpData.Rates.First().EffectiveDate, Value = nbpData.Rates.First().Mid });
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

                        rates.Add(new RateModel() { Date = nbpData.Rates.First().EffectiveDate, Value = nbpData.Rates.First().Mid });
                    }

                    return new CurrencyDataModel()
                    {
                        Code = nbpData.Code,
                        Rates = rates
                    };
                }

                case ExchangePlatform.YAHOO:
                {
                    string currencyCodeStr = currencyCode.ToString().ToUpper();

                    var rates = new List<RateModel>();

                    bool sameDates = !startTime.HasValue || !endTime.HasValue || startTime.Value == endTime.Value;

                    bool bothDatesProvided = startTime.HasValue && endTime.HasValue && !sameDates;
                    bool startDateProvided = startTime.HasValue;
                    bool noDateProvided = !startDateProvided && !bothDatesProvided;

                    if (bothDatesProvided)
                    {
                        try
                        {
                            var history = await Yahoo.GetHistoricalAsync($"{currencyCodeStr}PLN=X", startTime, endTime, Period.Daily);

                            foreach (var rate in history)
                            {
                                rates.Add(new RateModel() { Date = rate.DateTime, Value = (double)((rate.High + rate.Low) / 2) });
                            }
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                    else if (startDateProvided)
                    {
                        try
                        {
                            var history = await Yahoo.GetHistoricalAsync($"{currencyCodeStr}PLN=X", startTime, startTime, Period.Daily);
                            rates.Add(new RateModel() { Date = history.First().DateTime, Value = (double)((history.First().High + history.First().Low) / 2) });
                        }
                        catch (Exception e)
                        {
                            return null;
                        }
                    }
                    else if (noDateProvided)
                    {
                        try
                        {
                            var history = await Yahoo.GetHistoricalAsync($"{currencyCodeStr}PLN=X");
                            rates.Add(new RateModel() { Date = history.First().DateTime, Value = (double)((history.First().High + history.First().Low) / 2) });
                        }
                        catch (Exception e)
                        {
                            return null;
                        }
                    }

                    return new CurrencyDataModel()
                    {
                        Code = currencyCode.ToString(),
                        Rates = rates
                    };
                }

                case ExchangePlatform.EXCHANGERATE_HOST:
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

                            rates.Add(new RateModel() { Date = rate.Key, Value = value });
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

                        rates.Add(new RateModel() { Date = data.Rates.First().Key, Value = value });
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
        public static async Task<CurrencyDataModel> GetCryptocurrencyData(CryptoExchangePlatform cryptoPlatform, CryptocurrencyCode cryptocurrencyCode, CryptoInterval interval = CryptoInterval.ONE_DAY, DateTime? startTime = null, DateTime? endTime = null)
        {
            switch(cryptoPlatform)
            {
                case CryptoExchangePlatform.BINANCE:
                {
                    return await new BinanceDataRetriever(new BinanceClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(BinanceAPIKey, BinanceAPISecret),
                        AutoTimestamp = true,
                        AutoTimestampRecalculationInterval = TimeSpan.FromMinutes(30)
                    }).GetCurrencyData(cryptocurrencyCode, interval, startTime, endTime);
                }

                case CryptoExchangePlatform.BITFINEX:
                {
                    return await new BitfinexDataRetriever(new BitfinexClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(BitfinexAPIKey, BitfinexAPISecret)
                    }).GetCurrencyData(cryptocurrencyCode, interval, startTime, endTime);
                }

                default:
                return null;
            }
        }
        public static async Task<double> GetCryptoCurrentPrice(CryptoExchangePlatform cryptoPlatform, CryptocurrencyCode cryptocurrencyCode)
        {
            switch (cryptoPlatform)
            {
                case CryptoExchangePlatform.BINANCE:
                {
                    return await new BinanceDataRetriever(new BinanceClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(BinanceAPIKey, BinanceAPISecret),
                        AutoTimestamp = true,
                        AutoTimestampRecalculationInterval = TimeSpan.FromMinutes(30)
                    }).GetCurrentPrice(cryptocurrencyCode);
                }

                case CryptoExchangePlatform.BITFINEX:
                {
                    return await new BitfinexDataRetriever(new BitfinexClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(BitfinexAPIKey, BitfinexAPISecret)
                    }).GetCurrentPrice(cryptocurrencyCode);
                }

                default:
                    return 0;
            }
        }
    }
}
