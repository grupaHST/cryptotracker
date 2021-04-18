using ControlzEx.Theming;
using Cryptotracker.Backend;
using Cryptotracker.Backend.Generic;
using Cryptotracker.Backend.NBP;
using Cryptotracker.Languages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace Cryptotracker.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public Version AppVersion { get; } = Assembly.GetEntryAssembly().GetName().Version;

        public ObservableCollection<string> AvailableLanguages => new(Enum.GetNames<Language>());
        public Language Language { get; set; }

        public ThemeManager ThemeManager { get; } = ThemeManager.Current;

        public ObservableCollection<string> ExchangePlatforms => new(Enum.GetNames<ExchangePlatform>());
        public string SelectedExchangePlatform { get; set; }
        public ObservableCollection<GenericRate> Rates { get; set; }

        public ObservableCollection<string> CurrencyCodes => new(Enum.GetNames<CurrencyCode>());
        public string SelectedCurrencyCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ObservableCollection<string> Logs { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
