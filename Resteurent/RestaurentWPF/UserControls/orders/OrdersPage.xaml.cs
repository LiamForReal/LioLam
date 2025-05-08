using LiolamResteurent;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
            List<string> Labels= new List<string>();
            WebClient<Dictionary<DateTime, int>> client = new WebClient<Dictionary<DateTime, int>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetLineChart"
            };

            Dictionary<DateTime, int> data = await client.Get();

            DayOrderSeries.Clear();
            Labels.Clear();

            ChartValues<int> values = new ChartValues<int>();

            foreach (var item in data.OrderBy(d => d.Key))
            {
                values.Add(item.Value);
                Labels.Add(item.Key.ToString("dd/MM/yyyy"));
            }

            DayOrderSeries.Add(new LineSeries
            {
                Title = "Orders",
                Values = values
            });

            ordersLineChart.Series = DayOrderSeries;

            // Set the X axis labels
            ordersLineChart.AxisX.Clear();
            ordersLineChart.AxisX.Add(new Axis
            {
                Title = "Date",
                Labels = Labels
            });
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
            List<string> Labels = new List<string>();
            WebClient<Dictionary<DateTime, int>> client = new WebClient<Dictionary<DateTime, int>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/AreaChart"
            };

            Dictionary<DateTime, int> data = await client.Get();

            Labels.Clear();
            ProfitSeries.Clear();


            ChartValues<int> values = new ChartValues<int>();

            foreach (var item in data.OrderBy(d => d.Key))
            {
                values.Add(item.Value);
                Labels.Add(item.Key.ToString("dd/MM/yyyy"));
            }

            ProfitSeries.Add(new LineSeries
            {
                Title = "Profit",
                Values = values,
                Fill = Brushes.LightGreen,
                LineSmoothness = 0.5,
                PointGeometry = null
            });

            this.profitAreaChart.Series = ProfitSeries;

            profitAreaChart.AxisX.Clear();
            profitAreaChart.AxisX.Add(new Axis
            {
                Title = "Date",
                Labels = Labels
            });
        }
    }
}
