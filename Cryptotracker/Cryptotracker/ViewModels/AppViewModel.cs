using System;
using System.ComponentModel;
using System.Reflection;

namespace Cryptotracker.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public Version AppVersion { get; } = Assembly.GetEntryAssembly().GetName().Version;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
