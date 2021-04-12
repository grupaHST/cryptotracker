using Cryptotracker.Interfaces;
using System.IO;

namespace Cryptotracker.LocalData
{
    public class LocalFileLogger : IApplicationLogger
    {
        public static string FilePath => $"{LocalDataInfo.Directory}{Path.DirectorySeparatorChar}Logs.xaml";

        public void Log(string message)
        {
            // TODO: Log to local file, or create if file doesn't exists
        }
    }
}
