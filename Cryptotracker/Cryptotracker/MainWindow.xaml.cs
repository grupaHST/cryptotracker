using ControlzEx.Theming;
using Cryptotracker.Frontend.Converters;
using Cryptotracker.Languages;
using Cryptotracker.LocalData;
using Cryptotracker.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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

            var settings = LocalDataManager.Current.GetSettings();

            var appViewModel = DataContext as AppViewModel;

            string defaultConst = "Default";

            string language = string.IsNullOrEmpty(settings.Language) ? 
                TryFindResource($"{defaultConst}{nameof(settings.Language)}")?.ToString() : settings.Language;

            string baseColorScheme = string.IsNullOrEmpty(settings.BaseColorScheme) ? 
                TryFindResource($"{defaultConst}{nameof(settings.BaseColorScheme)}")?.ToString() : settings.BaseColorScheme;

            string colorScheme = string.IsNullOrEmpty(settings.ColorScheme) ? 
                TryFindResource($"{defaultConst}{nameof(settings.ColorScheme)}")?.ToString() : settings.ColorScheme;

            string startDate = string.IsNullOrEmpty(settings.StartDate) ? 
                TryFindResource($"{defaultConst}{nameof(settings.StartDate)}")?.ToString() : settings.StartDate;

            string endDate = string.IsNullOrEmpty(settings.EndDate) ? 
                TryFindResource($"{defaultConst}{nameof(settings.EndDate)}")?.ToString() : settings.EndDate;

            if (DateTime.TryParse(startDate, out DateTime sdate))
            {
                appViewModel.StartDate = sdate;
            }

            if (DateTime.TryParse(endDate, out DateTime edate))
            {
                appViewModel.EndDate = edate;
            }

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

        private async void Download(object sender, RoutedEventArgs e) => await App.DownloadAsync();

        private void LanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            var converter = TryFindResource(nameof(LanguageToCultureInfoConverter)) as LanguageToCultureInfoConverter;
            var cultureInfo = converter.Convert((DataContext as AppViewModel).Language, null, null, null) as CultureInfo;
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = cultureInfo;
        }
    }
}
