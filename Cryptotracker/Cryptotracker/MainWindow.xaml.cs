using MahApps.Metro.Controls;
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
