using LiolamResteurent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for Dishes.xaml
    /// </summary>
    public partial class DishesPage : UserControl
    {
        private List<Dish> dishes;
        static dishDetailsPage dishDetailsPage;
        public DishesPage()
        {
            InitializeComponent();
            GetAllDishes();
        }

        private async Task GetAllDishes()
        {
            WebClient<List<Dish>> client = new WebClient<List<Dish>>();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Manager/GetDishes";
            this.dishes = await client.Get();
            this.listView.ItemsSource = this.dishes;
        }

        private void inspectDish_Click(object sender, RoutedEventArgs e)
        {
            Button inspactButton = sender as Button;
            string dishId = inspactButton.Tag.ToString();
            dishDetailsPage = new dishDetailsPage(dishId);
            dishDetailsPage.Show();
        }

        private void updateDish_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteDish_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
