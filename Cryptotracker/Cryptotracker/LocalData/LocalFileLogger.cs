using Cryptotracker.Interfaces;
using System;
using System.IO;

namespace Cryptotracker.LocalData
{
    public class LocalFileLogger : IApplicationLogger
    {
        public static string FilePath => $"{LocalDataInfo.Directory}{Path.DirectorySeparatorChar}Logs.xaml";
        public string Error { get; private set; } = string.Empty;

        public void Log(string message)
        {
            try
            {
                using var writer = File.AppendText(FilePath);
                writer.WriteLine(message);
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
        }
    }
}
