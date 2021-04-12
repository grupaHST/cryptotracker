using System;
using System.Linq;

namespace Cryptotracker.LocalData
{
    public struct LocalAppSettings : IEquatable<LocalAppSettings>
    {
        public string Language { get; set; }
        public string BaseColorScheme { get; set; }
        public string ColorScheme { get; set; }

        public bool Equals(LocalAppSettings other)
        {
            var currentElement = this;

            return typeof(LocalAppSettings).GetProperties().ToList()
                .All(x => x.GetValue(currentElement) == x.GetValue(other));
        }

        public override bool Equals(object obj) => obj is LocalAppSettings settings && Equals(settings);
        public static bool operator ==(LocalAppSettings left, LocalAppSettings right) => left.Equals(right);
        public static bool operator !=(LocalAppSettings left, LocalAppSettings right) => !left.Equals(right);
        public override int GetHashCode() => base.GetHashCode();
    }
}
