﻿using ControlzEx.Theming;
using Cryptotracker.Backend.Generic;
using Cryptotracker.Backend.NBP;
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

        public ObservableCollection<string> AvailableLanguages => new(Enum.GetNames<Language>());
        public Language Language { get; set; }

        public ThemeManager ThemeManager { get; } = ThemeManager.Current;

        public ObservableCollection<GenericRate> Rates { get; set; }

        public ObservableCollection<string> CurrencyCodes => new(Enum.GetNames<CurrencyCode>());
        public string SelectedCurrencyCode { get; set; }
        public DateTime StartDate { get; set; } = new(2010, 01, 01);
        public DateTime EndDate { get; set; } = DateTime.Today;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
