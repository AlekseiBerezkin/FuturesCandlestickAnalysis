using Binance.Net;
using FuturesCandlestickAnalysis.Model;
using System;
using System.Collections.Generic;
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

namespace FuturesCandlestickAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        BinanceSocketClient socketClient = new BinanceSocketClient();
        Dictionary<string, Kline> newCandle60 = new Dictionary<string, Kline>();
        Dictionary<string, Kline> newCandle15 = new Dictionary<string, Kline>();
        Dictionary<string, Kline> newCandle = new Dictionary<string, Kline>();

        Dictionary<string, Kline> oldCandle60 = new Dictionary<string, Kline>();
        Dictionary<string, Kline> oldCandle15 = new Dictionary<string, Kline>();

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            GetPairs();
            StartWS();
        }

        private async void StartWS()
        {
            await socketClient.FuturesUsdt.SubscribeToKlineUpdatesAsync(newCandle.Keys, Binance.Net.Enums.KlineInterval.FifteenMinutes, r => 
            {
                if (!newCandle.ContainsKey(r.Data.Symbol))
                    return;
                if (!(r.Data.Data.CloseTime == newCandle[r.Data.Symbol].TimeClose))
                {
                    oldCandle15 = newCandle15;
                    newCandle15 = newCandle;
                    //метод который вычисляют патерн
                }
                if(r.Data.Symbol=="BTCUSDT")
                {
                    Dispatcher.Invoke(() => { lbRate15.Content = r.Data.Data.Close; });
                }
                newCandle[r.Data.Symbol] =new Kline {Close=r.Data.Data.Close,TimeClose=r.Data.Data.CloseTime,Open=r.Data.Data.Open};
            });

            await socketClient.FuturesUsdt.SubscribeToKlineUpdatesAsync("BTCUSDT", Binance.Net.Enums.KlineInterval.OneHour, r =>
            {
            
            });
        }

        private async void GetPairs()
        {
            await Task.Run((Action)(() => 
            {
                FileProvider fp = new FileProvider();
                List<string> pairs = fp.ReadFile();

                if(!pairs.Any())
                {
                    MessageBox.Show("Файл с парами не может быть прочитан или отсутствуют данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                foreach(string p in pairs)
                {
                    this.newCandle.Add(p, new Kline { });
                    this.newCandle60.Add(p, new Kline { });
                }
            }));
        }
    }
}
