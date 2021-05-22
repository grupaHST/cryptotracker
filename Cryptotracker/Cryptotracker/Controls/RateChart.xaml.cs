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
            OHLC [] ohlcs = new OHLC[(DataContext as AppViewModel).Rates.Count - 1];
            RateModel lastRate = (DataContext as AppViewModel).Rates[0];
            if  ((DataContext as AppViewModel).Rates[0].Low!=0)
            {
                for(int i = 1;i<(DataContext as AppViewModel).Rates.Count;i++)
                {
                    var tempRate = (DataContext as AppViewModel).Rates[i];
                    ohlcs[i - 1] = new OHLC(lastRate.Value, tempRate.High, tempRate.Low, tempRate.Value, tempRate.Date);
                    lastRate = tempRate;
                }
            }
            else
            {
                for (int i = 1; i < (DataContext as AppViewModel).Rates.Count; i++)
                {
                    var tempRate = (DataContext as AppViewModel).Rates[i];
                    ohlcs[i - 1] = new OHLC(lastRate.Value, lastRate.Value, tempRate.Value, tempRate.Value, tempRate.Date);
                    lastRate = tempRate;
                }
            }
            
            Chart.plt.Clear();
            Chart.plt.Title(String.Format("{0} Stock Chart",(DataContext as AppViewModel).SelectedCurrencyCode));
            Chart.plt.YLabel(String.Format("Stock Price ({0})", (DataContext as AppViewModel).SelectedCurrencyCode));
            Chart.plt.PlotCandlestick(ohlcs);
            Chart.plt.Ticks(dateTimeX: true);
            Chart.Render();
        }
    }
}
