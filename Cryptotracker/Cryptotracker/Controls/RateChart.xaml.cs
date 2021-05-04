using Cryptotracker.Backend.Generic;
using Cryptotracker.ViewModels;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            ScottPlot.OHLC[] ohlcs = DataGen.RandomStockPrices(rand: null, pointCount: 60, deltaMinutes: 10);
            Chart.plt.Title("Open/High/Low/Close (OHLC) Chart");
            Chart.plt.YLabel("Stock Price (USD)");
            Chart.plt.PlotOHLC(ohlcs);
            Chart.plt.Ticks(dateTimeX: true);

        }
        
    }
}
