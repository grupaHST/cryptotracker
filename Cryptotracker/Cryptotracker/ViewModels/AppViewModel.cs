using ControlzEx.Theming;
using Cryptotracker.Languages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace Cryptotracker.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public Version AppVersion { get; } = Assembly.GetEntryAssembly().GetName().Version;

        public ObservableCollection<string> AvailableLanguages => new(Enum.GetNames(typeof(Language)));
        public string DefaultLanguageString { get; set; } = Language.Polski.ToString();
        public Language Language { get; set; }

        public ThemeManager ThemeManager { get; } = ThemeManager.Current;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
