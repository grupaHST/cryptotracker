using ControlzEx.Theming;
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
        public void ChangeTheme(bool isDark) => ThemeManager.Current.ChangeThemeBaseColor(this, isDark ? "Dark" : "Light");

        public static void OpenWebPageInDefaultBrowser(string url)
        {
            try { Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); } catch { }
        }
    }
}
