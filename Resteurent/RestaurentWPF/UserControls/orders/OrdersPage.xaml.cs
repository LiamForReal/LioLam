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

namespace RestaurantWindowsPF.UserControls
{
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : UserControl
    {
        public SeriesCollection CustomerOrderSeries;
        public OrdersPage()
        {
            InitializeComponent();
            inishializeWindow();
        }

        private async Task inishializeWindow()
        {
            await CustomerPieChart();
            await OrdersLineChart();
            await DishesBarChart();
            await ProfitAriaChart();
        }

        private async Task CustomerPieChart()
        {
            if(CustomerOrderSeries == null)
            {
                WebClient<Dictionary<string, int>> client = new WebClient<Dictionary<string, int>>()
                {
                    Scheme = "https",
                    Port = 5125,
                    Host = "localhost",
                    Path = "api/Manager/GetPieChart"
                };

                Dictionary<string, int> data = await client.Get();
                foreach (var dictItem in data)
                    CustomerOrderSeries.Add(new PieSeries { Title = dictItem.Key, Values = new ChartValues<int> { dictItem.Value } });

                //this.CustomerPieChart = CustomerOrderSeries; download the correct librery
                
            }
           
            
            
        }

        private async Task OrdersLineChart()
        {

        }

        private async Task DishesBarChart()
        {

        }

        private async Task ProfitAriaChart()
        {

        }
    }
}
