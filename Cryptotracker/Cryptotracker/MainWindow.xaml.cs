using ControlzEx.Theming;
using Cryptotracker.Frontend.Converters;
using Cryptotracker.ViewModels;
using MahApps.Metro.Controls;
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

            var appViewModel = DataContext as AppViewModel;
            var theme = appViewModel.ThemeManager.DetectTheme();

            themeSwitch.IsOn = theme.BaseColorScheme == ThemeManager.BaseColorDark;
            languageSelector.SelectedValue = appViewModel.Language.ToString();
            colorSchemaSelector.SelectedValue = theme.ColorScheme;
        }

        private void ThemeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            (DataContext as AppViewModel).ThemeManager.ChangeThemeBaseColor
            (
                App.Current,
                themeSwitch.IsOn ? ThemeManager.BaseColorDark : ThemeManager.BaseColorLight
            );
        }

        private void ColorSchemaSelector_Selected(object sender, RoutedEventArgs e)
        {
            (DataContext as AppViewModel).ThemeManager.ChangeThemeColorScheme
            (
                App.Current,
                colorSchemaSelector.SelectedItem.ToString()
            );
        }

        private void OpenSettings(object sender, RoutedEventArgs e) => settingsFlyout.IsOpen = true;
        private void OpenKeyFlyout(object sender, RoutedEventArgs e) => keysFlyout.IsOpen = true;
        private void CloseSettings(object sender, MouseButtonEventArgs e)
        {
            settingsFlyout.IsOpen = false;
            keysFlyout.IsOpen = false;
        }

        private void LanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            var converter = TryFindResource(nameof(LanguageToCultureInfoConverter)) as LanguageToCultureInfoConverter;
            var cultureInfo = converter.Convert((DataContext as AppViewModel).Language, null, null, null) as CultureInfo;
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = cultureInfo;
        }
    }
}
