using LiolamResteurent;
using Microsoft.Build.Experimental;
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

namespace RestaurantWPF.UserControls
{
    /// <summary>
    /// Interaction logic for dishDetailsPage.xaml
    /// </summary>
    public partial class dishDetailsPage : Page
    {
        static Dish dish;
        public dishDetailsPage(string dishId)
        {
            InitializeComponent();
            getDishById(dishId);
            this.DataContext = dish;
        }

        private async Task getDishById(string id)
        {
            WebClient<Dish> client = new WebClient<Dish>();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Guest/GetSingleDish";
            client.AddParameter("id", id);
            dish = await client.Get();
        }
    }
}
