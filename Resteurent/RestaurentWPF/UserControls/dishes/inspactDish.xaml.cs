using LiolamResteurent;
using Microsoft.Build.Experimental;
using Microsoft.VisualBasic;
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
    /// Interaction logic for inspactDishPage.xaml
    /// </summary>
    public partial class InspactDish : Window
    {
        static Dish dish;
        public InspactDish(string dishId)
        {
            InitializeComponent();
            getDishById(dishId);
        }
        private async Task getDishById(string id)
        {
            WebClient<Dish> client = new WebClient<Dish>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Guest/GetSingleDish"
            };
            
            client.AddParameter("id", id);
            dish = await client.Get();

            this.DataContext = dish;
            
            this.priceLable.Content = $"{dish.DishPrice}₪";
        }
    }
}
