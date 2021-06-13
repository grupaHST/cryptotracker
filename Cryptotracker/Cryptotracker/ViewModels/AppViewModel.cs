using ControlzEx.Theming;
using Cryptotracker.Backend;
using Cryptotracker.Backend.Generic;
using Cryptotracker.Backend.Notifications;
using Cryptotracker.Languages;
using Cryptotracker.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Cryptotracker.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public Version AppVersion { get; } = Assembly.GetEntryAssembly().GetName().Version;

        public ObservableCollection<string> AvailableLanguages => new(Enum.GetNames<Language>());
        public Language Language { get; set; }

        public ThemeManager ThemeManager { get; } = ThemeManager.Current;

        public ObservableCollection<string> ExchangePlatforms => new(Enum.GetNames<ExchangePlatform>());
        public string SelectedExchangePlatform { get; set; }
        public ObservableCollection<string> CryptoExchangePlatforms => new(Enum.GetNames<CryptoExchangePlatform>());
        public string SelectedCryptoExchangePlatform { get; set; }

        public ObservableCollection<RateModel> Rates { get; set; }

        public ObservableCollection<string> CurrencyCodes => new(Enum.GetNames<CurrencyCode>());
        public string SelectedCurrencyCode { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsLoadingData { get; set; }

        public string FirstCurrencyCode { get; set; }
        public string FirstCurrencyValue { get; set; } = 0.ToString();
        public string SecondCurrencyCode { get; set; }
        public string SecondCurrencyValue { get; set; } = 0.ToString();

        public string BinanceKey
        {
            get => ExchangeRatesHandler.BinanceAPIKey;
            set => ExchangeRatesHandler.BinanceAPIKey = value;
        }
        public string BitfinexKey
        {
            get => ExchangeRatesHandler.BitfinexAPIKey;
            set => ExchangeRatesHandler.BitfinexAPIKey = value;
        }
        public string BinanceSecret
        {
            get => ExchangeRatesHandler.BinanceAPISecret;
            set => ExchangeRatesHandler.BinanceAPISecret = value;
        }
        public string BitfinexSecret
        {
            get => ExchangeRatesHandler.BitfinexAPISecret;
            set => ExchangeRatesHandler.BitfinexAPISecret = value;
        }

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
        
        public RelayCommand<string> SwapCurrenciesCommand => new(url =>
        {
            var tempCode = FirstCurrencyCode;
            var tempValue = FirstCurrencyValue;

            FirstCurrencyCode = SecondCurrencyCode;
            FirstCurrencyValue = SecondCurrencyValue;

            SecondCurrencyCode = tempCode;
            SecondCurrencyValue = tempValue;
        });

        public ObservableCollection<string> AvailableComparisions => new(Enum.GetNames<Comparison>());
        public CryptocurrencyCode NotificationCryptoCurrencyCode { get; set; }
        public CurrencyCode NotificationCurrencyCode { get; set; }
        public Comparison NotificationComparision { get; set; }
        public double NotificationThreeshold { get; set; }

        public RelayCommand AddCurrencyNotificationCommand => new(() =>
        {
            NotificationManager.AddNotification(new
            (
                NotificationCurrencyCode,
                Enum.Parse<ExchangePlatform>(SelectedExchangePlatform),
                NotificationThreeshold,
                Comparison.EQUAL
            ));
        });

        public RelayCommand AddCryptoCurrencyNotificationCommand => new(() =>
        {
            NotificationManager.AddNotification(new
            (
                NotificationCryptoCurrencyCode,
                Enum.Parse<CryptoExchangePlatform>(SelectedCryptoExchangePlatform),
                NotificationThreeshold,
                Comparison.EQUAL
            ));
        });

        public string Error { get; set; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(FirstCurrencyValue):
                        {
                            if (decimal.TryParse(FirstCurrencyValue, out decimal resultValue))
                            {
                                Error = null; // Brak błędu ponieważ konwersja tekstu na liczbę się powiodła
                                /*
                                 * Pierwsza waluta została zmodyfikowana przez usera, trzeba zmienić wartość drugiej waluty
                                 * SecondCurrencyValue = {resultValue po przeliczeniu}.ToString()
                                 * 
                                 * zmienne FirstCurrencyCode i SecondCurrencyCode to aktualnie wybrane przez usera kody walut
                                 */
                            }
                            else
                            {
                                Error = AppMessages.NumberConversionError(Language);
                            }
                        }
                        break;
                    case nameof(SecondCurrencyValue):
                        {
                            if (decimal.TryParse(SecondCurrencyValue, out decimal resultValue))
                            {
                                Error = null; // Brak błędu ponieważ konwersja tekstu na liczbę się powiodła
                                /*
                                 * Druga waluta została zmodyfikowana przez usera, trzeba zmienić wartość pierwszej waluty
                                 * FirstCurrencyValue = {resultValue po przeliczeniu}.ToString()
                                 * 
                                 * zmienne FirstCurrencyCode i SecondCurrencyCode to aktualnie wybrane przez usera kody walut
                                 */
                            }
                            else
                            {
                                Error = AppMessages.NumberConversionError(Language);
                            }
                        }
                        break;
                    default:
                        break;
                }
                return Error;
            }
        }
    }
}
