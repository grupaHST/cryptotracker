using Binance.Net.Enums;
using Bitfinex.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker
{
    //FOR EXTERNAL USE
    public enum CryptoInterval
    {
        ONE_MINUTE = 0,
        THREE_MINUTES = 1,
        FIVE_MINUTES = 2,
        FIFTEEN_MINUTES = 3,
        THIRTY_MINUTES = 4,
        ONE_HOUR = 5,
        TWO_HOURS = 6,
        THREE_HOURS = 7,
        FOUR_HOURS = 8,
        SIX_HOURS = 9,
        EIGHT_HOURS = 10,
        TWELVE_HOURS = 11,
        ONE_DAY = 12,
        THREE_DAYS = 13,
        ONE_WEEK = 14,
        TWO_WEEKS = 15,
        ONE_MONTH = 16
    }

    //FOR INTERNAL USE
    public static class CryptoIntervalsDict
    {
        public static readonly Dictionary<CryptoInterval, KlineInterval> UniToBinance = new Dictionary<CryptoInterval, KlineInterval>
        {
            { CryptoInterval.ONE_MINUTE, KlineInterval.OneMinute },
            { CryptoInterval.THREE_MINUTES, KlineInterval.ThreeMinutes },
            { CryptoInterval.FIVE_MINUTES, KlineInterval.FiveMinutes },
            { CryptoInterval.FIFTEEN_MINUTES, KlineInterval.FifteenMinutes },
            { CryptoInterval.THIRTY_MINUTES, KlineInterval.ThirtyMinutes },
            { CryptoInterval.ONE_HOUR, KlineInterval.OneHour },
            { CryptoInterval.TWO_HOURS, KlineInterval.TwoHour },
            { CryptoInterval.FOUR_HOURS, KlineInterval.FourHour },
            { CryptoInterval.SIX_HOURS, KlineInterval.SixHour },
            { CryptoInterval.EIGHT_HOURS, KlineInterval.EightHour },
            { CryptoInterval.TWELVE_HOURS, KlineInterval.TwelveHour },
            { CryptoInterval.ONE_DAY, KlineInterval.OneDay },
            { CryptoInterval.THREE_DAYS, KlineInterval.ThreeDay },
            { CryptoInterval.ONE_WEEK, KlineInterval.OneWeek },
            { CryptoInterval.ONE_MONTH, KlineInterval.OneMonth },
        };

        public static readonly Dictionary<CryptoInterval, TimeFrame> UniToBitfinex = new Dictionary<CryptoInterval, TimeFrame>
        {
            { CryptoInterval.ONE_MINUTE, TimeFrame.OneMinute },
            { CryptoInterval.FIVE_MINUTES, TimeFrame.FiveMinute },
            { CryptoInterval.FIFTEEN_MINUTES, TimeFrame.FifteenMinute },
            { CryptoInterval.THIRTY_MINUTES, TimeFrame.ThirtyMinute },
            { CryptoInterval.ONE_HOUR, TimeFrame.OneHour },
            { CryptoInterval.THREE_HOURS, TimeFrame.ThreeHour },
            { CryptoInterval.SIX_HOURS, TimeFrame.SixHour },
            { CryptoInterval.TWELVE_HOURS, TimeFrame.TwelveHour },
            { CryptoInterval.ONE_DAY, TimeFrame.OneDay },
            { CryptoInterval.ONE_WEEK, TimeFrame.SevenDay },
            { CryptoInterval.TWO_WEEKS, TimeFrame.FourteenDay },
            { CryptoInterval.ONE_MONTH, TimeFrame.OneMonth },
        };
    }
}
