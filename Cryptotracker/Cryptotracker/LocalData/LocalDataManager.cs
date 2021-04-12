using Cryptotracker.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace Cryptotracker.LocalData
{
    public class LocalDataManager : IDisposable
    {
        public static LocalDataManager Current { get; } = new();

        public static string FilePath => $"{LocalDataInfo.Directory}{Path.DirectorySeparatorChar}{nameof(AppSettings)}.xaml";

        public AppSettings Settings => new()
        {
            Language = GetSetting(nameof(AppSettings.Language)),
            BaseColorScheme = GetSetting(nameof(AppSettings.BaseColorScheme)),
            ColorScheme = GetSetting(nameof(AppSettings.ColorScheme)),
        };

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
                // TODO: Add application logs
                _document = EmptyDocumentStruct;
            }
        }

        public void Dispose()
        {
            try
            {
                var theme = _appViewModel.ThemeManager.DetectTheme();

                SetValue(nameof(AppSettings.Language), _appViewModel.Language.ToString());
                SetValue(nameof(AppSettings.BaseColorScheme), theme.BaseColorScheme);
                SetValue(nameof(AppSettings.ColorScheme), theme.ColorScheme);

                using FileStream stream = new(FilePath, FileMode.Truncate);
                _document.Save(stream);
            }
            catch (Exception)
            {
                // TODO: Add application logs
            }
        }

        private string GetSetting(string settingName)
        {
            return _document?.Descendants(settingName)?.FirstOrDefault(x => x.Name == settingName)?.Value;
        }

        private bool SetValue(string settingName, object newSettingValue)
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
