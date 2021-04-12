using ControlzEx.Theming;
using Cryptotracker.Interfaces;
using Cryptotracker.LocalData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
