using Cryptotracker.Interfaces;
using System.Windows;

namespace Cryptotracker.ViewModels
{
    public class AppViewModelLogger : IApplicationLogger
    {
        public void Log(string message)
        {
            ((Application.Current as App).TryFindResource(nameof(AppViewModel)) as AppViewModel)?.Logs?.Add(message);
        }
    }
}
