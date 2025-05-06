using LiolamResteurent;
using LiveCharts;
using LiveCharts.Wpf;
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
using WebApiClient;
using PieSeries = LiveCharts.Wpf.PieSeries;

namespace RestaurantWindowsPF.UserControls
{
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : UserControl
    {
        public SeriesCollection CustomerOrderSeries;
        public SeriesCollection DishSeries;
        public SeriesCollection DayOrderSeries;
        public SeriesCollection ProfitSeries;
        public OrdersPage()
        {
            InitializeComponent();

            CustomerOrderSeries = new SeriesCollection();
            DishSeries = new SeriesCollection();
            DayOrderSeries = new SeriesCollection();
            ProfitSeries = new SeriesCollection();

            inishializeWindow();
        }

        private async Task inishializeWindow()
        {
            await CustomerPieChart();
            await OrdersLineChart();
            await DishesBarChart();
            await ProfitAreaChart();
        }

        private async Task CustomerPieChart()
        {
            WebClient<Dictionary<string, int>> client = new WebClient<Dictionary<string, int>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetPieChart"
            };

            Dictionary<string, int> data = await client.Get();

            CustomerOrderSeries.Clear();

            foreach (var item in data)
            {
                CustomerOrderSeries.Add(new PieSeries
                {
                    Title = item.Key,
                    Values = new ChartValues<int> { item.Value },
                    Foreground = Brushes.Gold,
                });
            }

           this.customerPieChart.Series = CustomerOrderSeries;
        }

        private async Task OrdersLineChart()
        {
            WebClient<Dictionary<DateTime, int>> client = new WebClient<Dictionary<DateTime, int>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetLineChart"
            };

            Dictionary<DateTime, int> data = await client.Get();

            DayOrderSeries.Clear();

            foreach (var item in data)
            {
                DayOrderSeries.Add(new LineSeries
                {
                    Title = item.Key.ToString("YYYY-MM-DD"),
                    Values = new ChartValues<int> { item.Value }
                });
            }

            this.ordersLineChart.Series = DayOrderSeries;
        }

        private async Task DishesBarChart()
        {
            WebClient<Dictionary<string, int>> client = new WebClient<Dictionary<string, int>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/BarChart"
            };

            Dictionary<string, int> data = await client.Get();

            DishSeries.Clear();

            foreach (var item in data)
            {
                DishSeries.Add(new ColumnSeries
                {
                    Title = item.Key,
                    Values = new ChartValues<int> { item.Value }
                });
            }

            this.topDishesBarChart.Series = DishSeries;
        }

        private async Task ProfitAreaChart()
        {
            WebClient<Dictionary<DateTime, int>> client = new WebClient<Dictionary<DateTime, int>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/AreaChart"
            };

            Dictionary<DateTime, int> data = await client.Get();

            ProfitSeries.Clear();

            ProfitSeries.Add(new LineSeries
            {
                Title = "Profit",
                Values = new ChartValues<int>(data.Values),
                Fill = Brushes.LightGreen,
                LineSmoothness = 0.5,
                PointGeometry = null
            });

            this.profitAreaChart.Series = ProfitSeries;
        }
    }
}
