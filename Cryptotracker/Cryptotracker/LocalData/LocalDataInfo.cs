using System;
using System.IO;
using System.Reflection;

namespace Cryptotracker.LocalData
{
    public static class LocalDataInfo
    {
        public readonly static string Directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}" +
                                                  $"{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly().GetName().Name}" +
                                                  $"{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly().GetName().Version}";
    }
}
