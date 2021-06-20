using Cryptotracker.Backend.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Backend.FIAT.Yahoo
{
    public class YahooDataRetriever : CurrencyDataRetriever
    {
        public override async Task<CurrencyDataModel> GetCurrencyData(ExchangePlatform currencyPlatform, CurrencyCode currencyCode, DateTime? startTime = null, DateTime? endTime = null)
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
                    var history = await YahooFinanceApi.Yahoo.GetHistoricalAsync($"{currencyCodeStr}PLN=X", startTime, endTime, YahooFinanceApi.Period.Daily);

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
                    var history = await YahooFinanceApi.Yahoo.GetHistoricalAsync($"{currencyCodeStr}PLN=X", startTime, startTime, YahooFinanceApi.Period.Daily);
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
                    var history = await YahooFinanceApi.Yahoo.GetHistoricalAsync($"{currencyCodeStr}PLN=X");
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
    }
}
