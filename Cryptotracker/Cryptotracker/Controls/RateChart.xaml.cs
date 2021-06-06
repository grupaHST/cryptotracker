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
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(RateChart), 
                new PropertyMetadata("", new PropertyChangedCallback(OnDescriptionChanged)));

        public static readonly DependencyProperty CurrencyCodeProperty =
            DependencyProperty.Register("CurrencyCode", typeof(string), typeof(RateChart),
                new PropertyMetadata("", new PropertyChangedCallback(OnCurrencyCodeChanged)));

        public static readonly DependencyProperty YLabelProperty =
            DependencyProperty.Register("YLabel", typeof(string), typeof(RateChart),
                new PropertyMetadata("", new PropertyChangedCallback(OnDescriptionChanged)));

        public string Title
        {
            get {return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string CurrencyCode
        {
            get { return (string)GetValue(CurrencyCodeProperty); }
            set { SetValue(CurrencyCodeProperty, value); }
        }
        
        public string YLabel
        {
            get { return (string)GetValue(YLabelProperty); }
            set { SetValue(YLabelProperty, value); }
        }



        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RateChart rateChart = d as RateChart;
            rateChart.OnDescriptionChanged(e);
        }

        private void OnDescriptionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateChartDescription();
        }

        private static void OnCurrencyCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RateChart rateChart = d as RateChart;
            rateChart.OnCurrencyCodeChanged(e);
        }

        private void OnCurrencyCodeChanged(DependencyPropertyChangedEventArgs e)
        {
            Chart.Plot.Clear();
            UpdateChartDescription();
        }

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
                
        private void UpdateChartDescription()
        {
            Chart.Plot.Title(GenerateChartTitle());
            Chart.Plot.YLabel(GenerateChartYLabel());
            Chart.Render();
        }

        private string GenerateChartTitle()
        {
            return String.Format("{0} ({1})", Title, CurrencyCode);
        }

        private string GenerateChartYLabel()
        {
            return String.Format("{0} ({1})", YLabel, "PLN");
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
                    ohlcs[i - 1] = new OHLC(lastRate.Value, tempRate.High, tempRate.Low, tempRate.Value, tempRate.Date, tempRate.Date.Subtract(lastRate.Date));
                    lastRate = tempRate;
                }
            }
            else
            {
                for (int i = 1; i < (DataContext as AppViewModel).Rates.Count; i++)
                {
                    var tempRate = (DataContext as AppViewModel).Rates[i];
                    ohlcs[i - 1] = new OHLC(lastRate.Value, lastRate.Value, tempRate.Value, tempRate.Value, tempRate.Date, tempRate.Date.Subtract(lastRate.Date));
                    lastRate = tempRate;
                }
            }

            Chart.Plot.Clear();
            Chart.Plot.Title(GenerateChartTitle());
            Chart.Plot.YLabel(GenerateChartYLabel());
            Chart.Plot.AddCandlesticks(ohlcs);
            Chart.Plot.XAxis.DateTimeFormat(true);
            Chart.Render();
        }
    }
}
