using LiolamResteurent;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using WebApiClient;

namespace RestaurantWindowsPF.UserControls
{
    /// <summary>
    /// Interaction logic for updateDish.xaml
    /// </summary>
    public partial class UpdateDish : Window
    {
        private FileInfo readerPictureFile;
        private Dish loadedDish;
        public UpdateDish(string DishId)
        {
            InitializeComponent();
            this.errorLable.Content = "";
            setScreenByDishId(DishId);
        }

        private async Task setScreenByDishId(string id)
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
            string types = "";
            foreach (Category type in dish.types)
                types += type.TypeName + ", ";
            types = types.Substring(0, types.Length - 2);
            this.typesLable.Content = types;
            this.DataContext = dish;

            this.priceTextBox.Text = $"{dish.DishPrice}₪";

            loadedDish = new Dish()
            {
                Id = dish.Id,
                DishName = dish.DishName,
                DishDescription = dish.DishDescription,
                DishPrice = dish.DishPrice,
            };
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button updateButton = sender as Button;
                string dishId = updateButton.Tag.ToString();

                string price  = this.priceTextBox.Text;

                if (price.EndsWith("₪"))
                    price = price.Substring(0, price.Length - 1); // Remove ₪

                Dish dish = new Dish()
                {
                    Id = dishId,
                    DishName = this.nameTextBox.Text,
                    DishDescription = this.descriptionTextBox.Text,
                    DishPrice = int.Parse(price)
                };

                if (dish.DishName == "" || dish.DishDescription == "")
                {
                    errorLable.Content = "name or description cannot be empty";
                    return;
                }
                else if (dish.DishPrice <= 0)
                {
                    errorLable.Content = "price cannot be negative or 0";
                    return;
                }
                else if(loadedDish == dish && this.readerPictureFile == null)
                {
                    this.Close();
                }
                else updateDishDetails(dish, this.readerPictureFile);
            }
            catch(Exception ex)
            {
                this.errorLable.Content = "price must be an integer";
            }
        }

        private async Task updateDishDetails(Dish dish, FileInfo img = null)
        {
            WebClient<Dish> client = new WebClient<Dish>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/UpdateDish"
            };
            bool result;
            if (img == null)
                result = await client.Post(dish);
            else
            {
                dish.DishImage = img.Name;
                Stream imgStream = img.OpenRead();
                result = await client.Post(dish, imgStream);
            }
                

            if (result == true)
            {
                this.Close();
            }
            else
            {
                this.errorLable.Content = "update operation failed";
            }
        }

        private void picktureInput_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            // Specify the types of images which can be picked
            ofd.Filter = "Image files (*.png;*.jpeg;*.jpg;*.webp;*.jfif)|*.png;*.jpeg;*.jpg;*.webp;*.jiff";
            if (ofd.ShowDialog() == true)
            {
                this.readerPictureFile = new FileInfo(ofd.FileName);
                this.imgRenderer.Source = new BitmapImage(new Uri(this.readerPictureFile.FullName));
            }
        }
    }
}
