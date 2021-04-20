using System;
using System.Linq;

namespace Cryptotracker.LocalData
{
    public struct AppSettings : IEquatable<AppSettings>
    {
        public string Language { get; set; }
        public string BaseColorScheme { get; set; }
        public string ColorScheme { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SelectedCryptoExchangePlatform { get; set; }
        public string SelectedExchangePlatform { get; set; }
        public string SelectedCurrencyCode { get; set; }

        public bool Equals(AppSettings other)
        {
            var currentElement = this;

            return typeof(AppSettings).GetProperties().ToList()
                .All(x => x.GetValue(currentElement) == x.GetValue(other));
        }

        public override bool Equals(object obj) => obj is AppSettings settings && Equals(settings);
        public static bool operator ==(AppSettings left, AppSettings right) => left.Equals(right);
        public static bool operator !=(AppSettings left, AppSettings right) => !left.Equals(right);
        public override int GetHashCode() => base.GetHashCode();
    }
}
