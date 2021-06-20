
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Cryptotracker.Backend.Generic;
using Cryptotracker.Backend.ExchangeRateHost;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using Bitfinex.Net.Objects;
using Cryptotracker.Backend.Crypto.Bitfinex;
using Cryptotracker.Backend.Crypto.Binance;
using Cryptotracker.Backend.FIAT.NBP;
using Cryptotracker.Backend.FIAT.Yahoo;
using Cryptotracker.Backend.FIAT.ExchangeRateHost;

namespace Cryptotracker.Backend
{
    public static class ExchangeRatesHandler
    {
        public static string BinanceAPIKey { get; set; }
        public static string BinanceAPISecret { get; set; }

        public static string BitfinexAPIKey { get; set; }
        public static string BitfinexAPISecret { get; set; }

        public static async Task<CurrencyDataModel> GetFIATCurrencyData(ExchangePlatform currencyPlatform, CurrencyCode currencyCode, DateTime? startTime = null, DateTime? endTime = null)
        {
            CurrencyDataRetriever currencyDataRetriever = null;
            switch (currencyPlatform)
            {
                case ExchangePlatform.NBP:
                {
                    currencyDataRetriever = new NBPDataRetriever();
                }
                break;

                case ExchangePlatform.YAHOO:
                {
                    currencyDataRetriever = new YahooDataRetriever();
                }
                break;

                case ExchangePlatform.EXCHANGERATE_HOST:
                {
                    currencyDataRetriever = new ExchangeRateHostDataRetriever();
                }
                break;

                default:
                    return null;
            }

            return await currencyDataRetriever.GetCurrencyData(currencyPlatform, currencyCode, startTime, endTime);
        }
        public static async Task<double> GetFIATCurrentPrice(ExchangePlatform currencyPlatform, CurrencyCode currencyCode)
        {
            var resultTask = await GetFIATCurrencyData(currencyPlatform, currencyCode);
            return resultTask.Rates.First().Value;
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
            CryptocurrencyDataRetriever cryptocurrencyDataRetriever = null;
            switch(cryptoPlatform)
            {
                case CryptoExchangePlatform.BINANCE:
                {
                    cryptocurrencyDataRetriever = new BinanceDataRetriever(new BinanceClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(BinanceAPIKey, BinanceAPISecret),
                        AutoTimestamp = true,
                        AutoTimestampRecalculationInterval = TimeSpan.FromMinutes(30)
                    });
                }
                break;

                case CryptoExchangePlatform.BITFINEX:
                {
                    cryptocurrencyDataRetriever = new BitfinexDataRetriever(new BitfinexClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(BitfinexAPIKey, BitfinexAPISecret)
                    });
                }
                break;

                default:
                return null;
            }

            return await cryptocurrencyDataRetriever.GetCurrencyData(cryptocurrencyCode, interval, startTime, endTime);
        }

        public static async Task<double> GetCryptoCurrentPrice(CryptoExchangePlatform cryptoPlatform, CryptocurrencyCode cryptocurrencyCode)
        {
            CryptocurrencyDataRetriever cryptocurrencyDataRetriever = null;
            switch (cryptoPlatform)
            {
                case CryptoExchangePlatform.BINANCE:
                {
                    cryptocurrencyDataRetriever = new BinanceDataRetriever(new BinanceClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(BinanceAPIKey, BinanceAPISecret),
                        AutoTimestamp = true,
                        AutoTimestampRecalculationInterval = TimeSpan.FromMinutes(30)
                    });
                }
                break;

                case CryptoExchangePlatform.BITFINEX:
                {
                    cryptocurrencyDataRetriever = new BitfinexDataRetriever(new BitfinexClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(BitfinexAPIKey, BitfinexAPISecret)
                    });
                }
                break;

                default:
                    return 0;
            }

            return await cryptocurrencyDataRetriever.GetCurrentPrice(cryptocurrencyCode);
        }
    }
}
