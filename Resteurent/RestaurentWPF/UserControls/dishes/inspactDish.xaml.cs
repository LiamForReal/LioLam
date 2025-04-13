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
    public partial class inspactDish : Window
    {
        static Dish dish;
        public inspactDish(string dishId)
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

            string types = "";
            foreach (Category type in dish.types)
                types += type.TypeName + ", ";
            types = types.Substring(0, types.Length - 2);
            this.typesLable.Content = types;
            this.DataContext = dish;
            /*
            string messageBoxText = dish.chefs[0].ChefImage;
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            */
            this.priceLable.Content = $"{dish.DishPrice}₪";
        }
    }
}
