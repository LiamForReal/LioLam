using LiolamResteurent;
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
using System.Windows.Shapes;
using WebApiClient;

namespace RestaurantWindowsPF.UserControls
{
    /// <summary>
    /// Interaction logic for addDish.xaml
    /// </summary>
    public partial class addDish : Window
    {
        public addDish()
        {
            InitializeComponent();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Dish dish = new Dish()
            {
                DishName = this.nameTextBox.Text,
                DishDescription = this.descriptionTextBox.Text,
                DishPrice = int.Parse(this.priceTextBox.Text),
                //add dish image
            };
            addNewDish(dish);
        }

        private async Task addNewDish(Dish dish)
        {
            WebClient<Dish> client = new WebClient<Dish>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/AddNewDish"
            };

            bool result = await client.Post(dish);

            if(result)
            {
                this.Close();
            }
            else
            {
                //error
            }
        }
    }
}
