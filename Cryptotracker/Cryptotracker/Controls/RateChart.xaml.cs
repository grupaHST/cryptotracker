using Cryptotracker.Backend.Generic;
using Cryptotracker.ViewModels;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
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

        public static readonly DependencyProperty DarkModeProperty =
            DependencyProperty.Register("DarkMode", typeof(bool), typeof(RateChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnColorSchemeChanged)));

        public static readonly DependencyProperty ShowSMA1Property =
            DependencyProperty.Register("ShowSMA1", typeof(bool), typeof(RateChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIndicatorChanged)));

        public static readonly DependencyProperty SMA1_NProperty =
            DependencyProperty.Register("SMA1_N", typeof(int), typeof(RateChart),
                new PropertyMetadata(20, new PropertyChangedCallback(OnIndicatorChanged)));

        public static readonly DependencyProperty ShowSMA2Property =
            DependencyProperty.Register("ShowSMA2", typeof(bool), typeof(RateChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIndicatorChanged)));

        public static readonly DependencyProperty SMA2_NProperty =
            DependencyProperty.Register("SMA2_N", typeof(int), typeof(RateChart),
                new PropertyMetadata(8, new PropertyChangedCallback(OnIndicatorChanged)));

        public static readonly DependencyProperty ShowBollingerBandsProperty =
            DependencyProperty.Register("ShowBollingerBands", typeof(bool), typeof(RateChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIndicatorChanged)));

        public static readonly DependencyProperty BollingerBandsNProperty =
            DependencyProperty.Register("BollingerBandsN", typeof(int), typeof(RateChart),
                new PropertyMetadata(8, new PropertyChangedCallback(OnIndicatorChanged)));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
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

        public bool DarkMode
        {
            get { return (bool)GetValue(DarkModeProperty); }
            set { SetValue(DarkModeProperty, value); }
        }

        public bool ShowSMA1
        {
            get { return (bool)GetValue(ShowSMA1Property); }
            set { SetValue(ShowSMA1Property, value); }
        }

        public int SMA1_N
        {
            get { return (int)GetValue(SMA1_NProperty); }
            set { SetValue(SMA1_NProperty, value); }
        }

        public bool ShowSMA2
        {
            get { return (bool)GetValue(ShowSMA2Property); }
            set { SetValue(ShowSMA2Property, value); }
        }

        public int SMA2_N
        {
            get { return (int)GetValue(SMA2_NProperty); }
            set { SetValue(SMA2_NProperty, value); }
        }

        public bool ShowBollingerBands
        {
            get { return (bool)GetValue(ShowBollingerBandsProperty); }
            set { SetValue(ShowBollingerBandsProperty, value); }
        }

        public int BollingerBandsN
        {
            get { return (int)GetValue(BollingerBandsNProperty); }
            set { SetValue(BollingerBandsNProperty, value); }
        }

        private FinancePlot candlePlot;
        private ScatterPlot sma1;
        private ScatterPlot sma2;
        private ScatterPlot bollingerSma;
        private ScatterPlot bollingerLower;
        private ScatterPlot bollingerUpper;


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

        private static void OnColorSchemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RateChart rateChart = d as RateChart;
            rateChart.OnColorSchemeChanged(e);
        }

        private void OnColorSchemeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateColorScheme();
        }

        private static void OnIndicatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RateChart rateChart = d as RateChart;
            rateChart.OnIndicatorChanged(e);
        }

        private void OnIndicatorChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateIndicators();
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

        private void UpdateColorScheme()
        {
            if (DarkMode)
            {
                Chart.Plot.Style(figureBackground: ColorTranslator.FromHtml("#0d0d0d"),
                                dataBackground: ColorTranslator.FromHtml("#0d0d0d"),
                                titleLabel: System.Drawing.Color.White,
                                axisLabel: System.Drawing.Color.White);

                Chart.Plot.XAxis.TickLabelStyle(color: System.Drawing.Color.White);
                Chart.Plot.XAxis.TickMarkColor(ColorTranslator.FromHtml("#595959"));
                Chart.Plot.XAxis.MajorGrid(color: ColorTranslator.FromHtml("#595959"));

                Chart.Plot.YAxis.TickLabelStyle(color: System.Drawing.Color.White);
                Chart.Plot.YAxis.TickMarkColor(ColorTranslator.FromHtml("#595959"));
                Chart.Plot.YAxis.MajorGrid(color: ColorTranslator.FromHtml("#595959"));
            }
            else
            {
                Chart.Plot.Style(figureBackground: System.Drawing.Color.White,
                                dataBackground: System.Drawing.Color.White,
                                titleLabel: System.Drawing.Color.Black,
                                axisLabel: System.Drawing.Color.Black);

                Chart.Plot.XAxis.TickLabelStyle(color: System.Drawing.Color.Black);
                Chart.Plot.XAxis.TickMarkColor(ColorTranslator.FromHtml("#e6e6e6"));
                Chart.Plot.XAxis.MajorGrid(color: ColorTranslator.FromHtml("#e6e6e6"));

                Chart.Plot.YAxis.TickLabelStyle(color: System.Drawing.Color.Black);
                Chart.Plot.YAxis.TickMarkColor(ColorTranslator.FromHtml("#e6e6e6"));
                Chart.Plot.YAxis.MajorGrid(color: ColorTranslator.FromHtml("#e6e6e6"));
            }

            ClearIndicators();
            Chart.Render();
            UpdateIndicators();
        }

        private void UpdateIndicators()
        {
            if (ShowSMA1 && SMA1_N>1)
            {
                Chart.Plot.Remove(sma1);
                var sma1Data = candlePlot.GetSMA(SMA1_N);
                sma1 = Chart.Plot.AddScatterLines(sma1Data.xs, sma1Data.ys, System.Drawing.Color.Cyan, 2, label: "SMA1");
            }
            else
            {
                Chart.Plot.Remove(sma1);
            }
            
            if (ShowSMA2 && SMA2_N > 1)
            {
                Chart.Plot.Remove(sma2);
                var sma2Data = candlePlot.GetSMA(SMA2_N);
                sma2 = Chart.Plot.AddScatterLines(sma2Data.xs, sma2Data.ys, System.Drawing.Color.Orange, 2, label: "SMA2");
            }
            else
            {
                Chart.Plot.Remove(sma2);
            }

            if (ShowBollingerBands && BollingerBandsN > 1)
            {
                Chart.Plot.Remove(bollingerSma);
                Chart.Plot.Remove(bollingerLower);
                Chart.Plot.Remove(bollingerUpper);
                var bollingerData = candlePlot.GetBollingerBands(BollingerBandsN);
                bollingerSma = Chart.Plot.AddScatterLines(bollingerData.xs, bollingerData.sma, System.Drawing.Color.DarkCyan, 2);
                bollingerLower = Chart.Plot.AddScatterLines(bollingerData.xs, bollingerData.lower, System.Drawing.Color.Cyan, lineStyle: LineStyle.Dash);
                bollingerUpper = Chart.Plot.AddScatterLines(bollingerData.xs, bollingerData.upper, System.Drawing.Color.Cyan, lineStyle: LineStyle.Dash);
            }
            else
            {
                Chart.Plot.Remove(bollingerSma);
                Chart.Plot.Remove(bollingerLower);
                Chart.Plot.Remove(bollingerUpper);
            }
        }

        private void ClearIndicators()
        {
            Chart.Plot.Remove(sma1);
            Chart.Plot.Remove(sma2);
            Chart.Plot.Remove(bollingerSma);
            Chart.Plot.Remove(bollingerLower);
            Chart.Plot.Remove(bollingerUpper);
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
                    ohlcs[i - 1] = new OHLC(lastRate.Value, 
                                            tempRate.High, 
                                            tempRate.Low, 
                                            tempRate.Value, 
                                            tempRate.Date, 
                                            tempRate.Date.Subtract(lastRate.Date));
                    lastRate = tempRate;
                }
            }
            else
            {
                for (int i = 1; i < (DataContext as AppViewModel).Rates.Count; i++)
                {
                    var tempRate = (DataContext as AppViewModel).Rates[i];
                    ohlcs[i - 1] = new OHLC(lastRate.Value, 
                                            lastRate.Value, 
                                            tempRate.Value, 
                                            tempRate.Value, 
                                            tempRate.Date, 
                                            tempRate.Date.Subtract(lastRate.Date));
                    lastRate = tempRate;
                }
            }

            Chart.Plot.Clear();
            Chart.Plot.Title(GenerateChartTitle());
            Chart.Plot.YLabel(GenerateChartYLabel());
            candlePlot = Chart.Plot.AddCandlesticks(ohlcs);
            Chart.Plot.XAxis.DateTimeFormat(true);

            UpdateIndicators();

            Chart.Render();

        }
    }
}
