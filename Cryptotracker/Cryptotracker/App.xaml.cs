using ControlzEx.Theming;
using Cryptotracker.Backend;
using Cryptotracker.Backend.NBP;
using Cryptotracker.Interfaces;
using Cryptotracker.Languages;
using Cryptotracker.LocalData;
using Cryptotracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace Cryptotracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void ChangeColorScheme(string colorSchema) => ThemeManager.Current.ChangeThemeColorScheme(this, colorSchema);
        public void ChangeBaseColorScheme(bool isDark)
        {
            ThemeManager.Current.ChangeThemeBaseColor(this, isDark ? ThemeManager.BaseColorDark : ThemeManager.BaseColorLight);
        }

        public void LogMessage(string message) => _loggers.ForEach(x => x.Log($"[{DateTime.Now}]: {message}"));

        public static async Task DownloadAsync()
        {
            if (Current.TryFindResource(nameof(AppViewModel)) is AppViewModel viewModel)
            {
                try
                {
                    viewModel.IsLoadingData = true;

                    var genericCurrencyData = await ExchangeRatesHandler.GetCurrencyData
                    (
                        Enum.Parse<ExchangePlatform>(viewModel.SelectedExchangePlatform),
                        Enum.Parse<CurrencyCode>(viewModel.SelectedCurrencyCode),
                        viewModel.StartDate,
                        viewModel.EndDate
                    );

                    viewModel.Rates = new(genericCurrencyData.Rates);
                }
                catch (Exception e)
                {
                    (Current as App).LogMessage(e.Message);
                }
                finally
                {
                    viewModel.IsLoadingData = false;
                }
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _loggers = new()
            {
                new AppViewModelLogger(),
                new LocalFileLogger()
            };

            object Default(object key) => TryFindResource($"Default{key}");
            var vm = TryFindResource(nameof(AppViewModel)) as AppViewModel;

            var crypto = Cryptotracker.Properties.Settings.Default[nameof(vm.SelectedCryptoExchangePlatform)]?.ToString();
            vm.SelectedCryptoExchangePlatform = string.IsNullOrEmpty(crypto) ?
                Default(nameof(vm.SelectedCryptoExchangePlatform))?.ToString() : crypto;

            var exchange = Cryptotracker.Properties.Settings.Default[nameof(vm.SelectedExchangePlatform)]?.ToString();
            vm.SelectedExchangePlatform = string.IsNullOrEmpty(exchange) ?
                Default(nameof(vm.SelectedExchangePlatform))?.ToString() : exchange;

            var currencyCode = Cryptotracker.Properties.Settings.Default[nameof(vm.SelectedCurrencyCode)]?.ToString();
            vm.SelectedCurrencyCode = string.IsNullOrEmpty(currencyCode)
                ? Default(nameof(vm.SelectedCurrencyCode))?.ToString() : currencyCode;

            var startDate = Cryptotracker.Properties.Settings.Default[nameof(vm.StartDate)]?.ToString();
            bool parseResult = DateTime.TryParse
            (
                string.IsNullOrEmpty(startDate) ? Default(nameof(vm.StartDate))?.ToString() : startDate,
                out DateTime parsedStartDate
            );
            vm.StartDate = parseResult ? parsedStartDate : default;

            var endDate = Cryptotracker.Properties.Settings.Default[nameof(vm.EndDate)]?.ToString();
            parseResult = DateTime.TryParse
            (
                string.IsNullOrEmpty(endDate) ? Default(nameof(vm.EndDate))?.ToString() : endDate,
                out DateTime parsedEndDate
            );
            vm.EndDate = parseResult ? parsedEndDate : default;

            var language = Cryptotracker.Properties.Settings.Default[nameof(vm.Language)]?.ToString();
            parseResult = Enum.TryParse
            (
                string.IsNullOrEmpty(language) ? Default(nameof(vm.Language)).ToString() : language,
                out Language parsedLanguage
            );
            vm.Language = parseResult ? parsedLanguage : default;

            string setting = Cryptotracker.Properties.Settings.Default[nameof(Theme.BaseColorScheme)]?.ToString();
            var baseColor = string.IsNullOrEmpty(setting) ? Default(nameof(Theme.BaseColorScheme))?.ToString() : setting;

            if (vm.ThemeManager.BaseColors.Contains(baseColor))
            {
                bool isDark = baseColor == ThemeManager.BaseColorDark;
                ChangeBaseColorScheme(isDark);
            }

            setting = Cryptotracker.Properties.Settings.Default[nameof(Theme.ColorScheme)]?.ToString();
            var color = string.IsNullOrEmpty(setting) ? Default(nameof(Theme.ColorScheme))?.ToString() : setting;

            if (vm.ThemeManager.ColorSchemes.Contains(color))
            {
                ChangeColorScheme(color);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var viewModel = TryFindResource(nameof(AppViewModel)) as AppViewModel;
            var theme = viewModel.ThemeManager.DetectTheme();
            Cryptotracker.Properties.Settings.Default.BaseColorScheme = theme.BaseColorScheme;
            Cryptotracker.Properties.Settings.Default.ColorScheme = theme.ColorScheme;
            Cryptotracker.Properties.Settings.Default.StartDate = viewModel.StartDate;
            Cryptotracker.Properties.Settings.Default.EndDate = viewModel.EndDate;
            Cryptotracker.Properties.Settings.Default.SelectedCryptoExchangePlatform = viewModel.SelectedCryptoExchangePlatform;
            Cryptotracker.Properties.Settings.Default.SelectedExchangePlatform = viewModel.SelectedExchangePlatform;
            Cryptotracker.Properties.Settings.Default.SelectedCurrencyCode = viewModel.SelectedCurrencyCode;
            Cryptotracker.Properties.Settings.Default.Save();
        }

        protected List<IApplicationLogger> _loggers;
    }
}
