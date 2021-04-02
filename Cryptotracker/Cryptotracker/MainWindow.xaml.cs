using MahApps.Metro.Controls;
using System.Windows;

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

        private void SettingsOpener_Click(object sender, RoutedEventArgs e) => settingsFlyout.IsOpen = true;
    }
}
