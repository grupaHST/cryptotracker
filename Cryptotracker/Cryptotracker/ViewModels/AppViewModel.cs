using ControlzEx.Theming;
using Cryptotracker.Backend;
using Cryptotracker.Backend.Generic;
using Cryptotracker.Backend.NBP;
using Cryptotracker.Languages;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        public ObservableCollection<string> CryptoExchangePlatforms => new(Enum.GetNames<CryptoExchangePlatform>());
        public string SelectedCryptoExchangePlatform { get; set; }

        public ObservableCollection<GenericRate> Rates { get; set; }

        public ObservableCollection<string> CurrencyCodes => new(Enum.GetNames<CurrencyCode>());
        public string SelectedCurrencyCode { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsLoadingData { get; set; }

        public ObservableCollection<string> Logs { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand DownloadCommand => new(async() =>
        {
            try
            {
                IsLoadingData = true;

                var genericCurrencyData = await ExchangeRatesHandler.GetCurrencyData
                (
                    Enum.Parse<ExchangePlatform>(SelectedExchangePlatform),
                    Enum.Parse<CurrencyCode>(SelectedCurrencyCode),
                    StartDate,
                    EndDate
                );

                Rates = new(genericCurrencyData?.Rates);
            }
            catch (Exception e)
            {
                (App.Current as App).LogMessage(e.Message);
            }
            finally
            {
                IsLoadingData = false;
            }
        });

        public RelayCommand<string> OpenInBrowserCommand => new(url =>
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception e)
            {
                (App.Current as App).LogMessage(e.Message);
            }
        });
    }
}
