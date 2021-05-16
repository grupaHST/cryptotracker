using Cryptotracker.Backend.Generic;
using Cryptotracker.ViewModels;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cryptotracker.Controls
{
    public partial class RateChart : UserControl
    {
        public RateChart()
        {
            InitializeComponent();

            (DataContext as AppViewModel).PropertyChanged += AppViewModel_PropertyChanged;

        }

        private void AppViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Rates")
            {
                UpdateChart();
            }
        }

        private void UpdateChart()
        {
            List<OHLC> tempOhlcs = new List<OHLC>();
            GenericRate lastRate = null;
            foreach (var rate in (DataContext as AppViewModel).Rates)
            {
                if (lastRate != null)
                {
                    tempOhlcs.Add(new OHLC(lastRate.Value, lastRate.Value, rate.Value, rate.Value, rate.Date));
                }
                lastRate = rate;
                
            }

            var ohlcs = tempOhlcs.ToArray();
            Chart.plt.Clear();
            Chart.plt.Title(String.Format("{0} Stock Chart",(DataContext as AppViewModel).SelectedCurrencyCode));
            Chart.plt.YLabel(String.Format("Stock Price ({0})", (DataContext as AppViewModel).SelectedCurrencyCode));
            Chart.plt.PlotCandlestick(ohlcs);
            Chart.plt.Ticks(dateTimeX: true);
            Chart.Render();
        }
    }
}
