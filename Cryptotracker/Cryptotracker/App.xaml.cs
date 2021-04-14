using ControlzEx.Theming;
using Cryptotracker.Backend;
using Cryptotracker.Backend.NBP;
using Cryptotracker.Interfaces;
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
        public void ChangeColorSchema(string colorSchema) => ThemeManager.Current.ChangeThemeColorScheme(this, colorSchema);
        public void ChangeTheme(bool isDark)
        {
            ThemeManager.Current.ChangeThemeBaseColor(this, isDark ? ThemeManager.BaseColorDark : ThemeManager.BaseColorLight);
        }

        public void LogMessage(string message) => _loggers.ForEach(x => x.Log($"[{DateTime.Now}]: {message}"));

        public static void OpenWebPageInDefaultBrowser(string url)
        {
            try { Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); } catch { }
        }

        public static async Task DownloadAsync()
        {
            if (Current.TryFindResource(nameof(AppViewModel)) is AppViewModel viewModel)
            {
                try
                {
                    var genericCurrencyData = await ExchangeRatesHandler.GetCurrencyData
                    (
                        ExchangePlatform.NBP,
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
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // application loggers

            _loggers = new()
            {
                new LocalFileLogger(),

            };

            LocalDataManager.Current.Init();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LocalDataManager.Current.Dispose();
        }

        protected List<IApplicationLogger> _loggers;
    }
}
