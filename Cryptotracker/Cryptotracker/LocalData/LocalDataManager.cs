using Cryptotracker.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Linq;

namespace Cryptotracker.LocalData
{
    public class LocalDataManager : IDisposable
    {
        public static LocalDataManager Current { get; } = new();

        public static string FilePath => $"{LocalDataInfo.Directory}{Path.DirectorySeparatorChar}{nameof(AppSettings)}.xaml";

        public AppSettings GetSettings()
        {
            object boxedObject = RuntimeHelpers.GetObjectValue(new AppSettings());
            typeof(AppSettings).GetProperties().ToList().ForEach(x => x.SetValue(boxedObject, GetSetting(x.Name)));
            return (AppSettings)boxedObject;
        }

        public static XDocument EmptyDocumentStruct => new(new XElement
        (
            nameof(Cryptotracker),
            typeof(AppSettings).GetProperties().Select(x => new XElement(x.Name))
        ));

        public void Init()
        {
            try
            {
                DirectoryInfo dir = new(LocalDataInfo.Directory);
                _appViewModel = Application.Current.FindResource(nameof(AppViewModel)) as AppViewModel;

                if (!dir.Exists)
                {
                    dir.Create();
                }

                using FileStream stream = new(FilePath, FileMode.OpenOrCreate);
                _document = XDocument.Load(stream);
            }
            catch (Exception e)
            {
                (Application.Current as App).LogMessage(e.Message);
                _document = EmptyDocumentStruct;
            }
        }

        public void Dispose()
        {
            try
            {
                var theme = _appViewModel.ThemeManager.DetectTheme();

                SetSetting(nameof(AppSettings.Language), _appViewModel.Language.ToString());
                SetSetting(nameof(AppSettings.BaseColorScheme), theme.BaseColorScheme);
                SetSetting(nameof(AppSettings.ColorScheme), theme.ColorScheme);
                SetSetting(nameof(AppSettings.StartDate), _appViewModel.StartDate.ToShortDateString());
                SetSetting(nameof(AppSettings.EndDate), _appViewModel.EndDate.ToShortDateString());
                SetSetting(nameof(AppSettings.SelectedCryptoExchangePlatform), _appViewModel.SelectedCryptoExchangePlatform);
                SetSetting(nameof(AppSettings.SelectedExchangePlatform), _appViewModel.SelectedExchangePlatform);

                using FileStream stream = new(FilePath, FileMode.Truncate);
                _document.Save(stream);
            }
            catch (Exception e)
            {
                (Application.Current as App).LogMessage(e.Message);
            }
        }

        private string GetSetting(string settingName)
        {
            return _document?.Descendants(settingName)?.FirstOrDefault(x => x.Name == settingName)?.Value;
        }

        private bool SetSetting(string settingName, object newSettingValue)
        {
            var setting = _document?.Descendants(settingName)?.FirstOrDefault(x => x.Name == settingName);

            if (setting is not null)
            {
                setting.Value = newSettingValue.ToString();
                return true;
            }

            return false;
        }

        private LocalDataManager()
        {
            
        }

        private AppViewModel _appViewModel;
        private XDocument _document;
    }
}
