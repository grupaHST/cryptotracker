using ControlzEx.Theming;
using Cryptotracker.Languages;
using Cryptotracker.LocalData;
using Cryptotracker.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Input;

namespace Cryptotracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public override void EndInit()
        {
            base.EndInit();

            var settings = LocalDataManager.Current.Settings;
            var appViewModel = DataContext as AppViewModel;

            string language = string.IsNullOrEmpty(settings.Language) ? 
                TryFindResource($"Default{nameof(settings.Language)}")?.ToString() : settings.Language;

            string baseColorScheme = string.IsNullOrEmpty(settings.BaseColorScheme) ? 
                TryFindResource($"Default{nameof(settings.BaseColorScheme)}")?.ToString() : settings.BaseColorScheme;

            string colorScheme = string.IsNullOrEmpty(settings.ColorScheme) ? 
                TryFindResource($"Default{nameof(settings.ColorScheme)}")?.ToString() : settings.ColorScheme;

            if (Enum.TryParse(language, out Language enumLang))
            {
                appViewModel.Language = enumLang;
                languageSelector.SelectedValue = enumLang.ToString();
            }

            if (appViewModel.ThemeManager.BaseColors.Contains(baseColorScheme))
            {
                bool isDark = baseColorScheme == ThemeManager.BaseColorDark;
                themeSwitch.IsOn = isDark;
                (Application.Current as App).ChangeTheme(isDark);
            }

            if (appViewModel.ThemeManager.ColorSchemes.Contains(colorScheme))
            {
                colorSchemaSelector.SelectedValue = colorScheme;
                (Application.Current as App).ChangeColorSchema(colorScheme);
            }
        }

        private void ThemeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).ChangeTheme(themeSwitch.IsOn);
        }

        private void ColorSchemaSelector_Selected(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).ChangeColorSchema(colorSchemaSelector.SelectedItem.ToString());
        }

        private void HyperlinkClick(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is string url)
            {
                App.OpenWebPageInDefaultBrowser(url);
            }
        }

        private void OpenSettings(object sender, RoutedEventArgs e) => settingsFlyout.IsOpen = true;
        private void CloseSettings(object sender, MouseButtonEventArgs e) => settingsFlyout.IsOpen = false;
    }
}
