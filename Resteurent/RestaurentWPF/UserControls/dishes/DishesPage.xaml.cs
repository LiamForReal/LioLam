using LiolamResteurent;
using RestaurantWindowsPF.UserControls;
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
        static inspactDish inspactDishPage;
        static updateDish updateDishPage;
        static addDish addDishPage;
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
            inspactDishPage = new inspactDish(dishId);
            inspactDishPage.ShowDialog();
        }

        private void updateDish_Click(object sender, RoutedEventArgs e)
        {
            Button updateButton = sender as Button;
            string dishId = updateButton.Tag.ToString();
            updateDishPage = new updateDish(dishId);
            updateDishPage.ShowDialog();
            GetAllDishes();
        }

        private async void deleteDish_Click(object sender, RoutedEventArgs e)
        {
            Button updateButton = sender as Button;
            string dishId = updateButton.Tag.ToString();
            string dishName = await getDishNameById(dishId);
            string messageBoxText = $"Confirm delete: '{dishName}'?";
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.None;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                deleteDish(dishId);
                GetAllDishes();
            }
        }

        private async Task deleteDish(string dishId)
        {
            WebClient<string> client = new WebClient<string>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/DeleteDish"
            };

            bool result = await client.Post(dishId);

            if(!result)
            {
                //error
            }
        }
        private async Task<string> getDishNameById(string id)
        {
            WebClient<Dish> client = new WebClient<Dish>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Guest/GetSingleDish"
            };
            
            client.AddParameter("id", id);
            Dish dish = await client.Get();
            return dish.DishName;
        }
        private void addNewDish_Click(object sender, RoutedEventArgs e)
        {
            addDishPage = new addDish();
            addDishPage.ShowDialog(); 
            GetAllDishes();
        }
    }
}
