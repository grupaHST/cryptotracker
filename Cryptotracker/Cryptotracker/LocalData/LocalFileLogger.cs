using Cryptotracker.Interfaces;
using System.IO;

namespace Cryptotracker.LocalData
{
    public class LocalFileLogger : IApplicationLogger
    {
        public static string FilePath => $"{LocalDataInfo.Directory}{Path.DirectorySeparatorChar}Logs.xaml";

        public void Log(string message)
        {
            using var writer = File.AppendText(FilePath);
            writer.WriteLine(message);
        }
    }
}
