using LiolamResteurent;
using Microsoft.Win32;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        private FileInfo readerPictureFile;
        public addDish()
        {
            InitializeComponent();
            this.errorLable.Content = "";
            SetScreenByChefsAndTypes();
        }

        private async Task SetScreenByChefsAndTypes()
        {
            WebClient<AddDishView> client = new WebClient<AddDishView>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetAddDishView"
            };

            AddDishView addDishView = await client.Get();

            this.DataContext = addDishView;
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.errorLable.Content = "";
                string price = this.priceTextBox.Text;

                if (price.EndsWith("₪"))
                    price = price.Substring(0, price.Length - 1); // Remove ₪

                Dish dish = new Dish()
                {
                    DishName = this.nameTextBox.Text,
                    DishDescription = this.descriptionTextBox.Text,
                    DishPrice = int.Parse(price),
                    types = this.types.SelectedItems.Cast<Category>().ToList(),
                    chefs = this.chefs.SelectedItems.Cast<Chef>().ToList()
                };

                if(dish.DishName == "" || dish.DishDescription == "")
                {
                    errorLable.Content = "you have to fill dish name and description";
                    return;
                }
                else if(dish.DishPrice <= 0)
                {
                    errorLable.Content = "price cannot be negative or 0";
                    return;
                }
                else if(dish.types.Count < 1 || dish.chefs.Count < 1)
                {
                    errorLable.Content = "every dish must contains at list one chef and type";
                    return;
                }
                addNewDish(dish, this.readerPictureFile);
            }
            catch (Exception ex)
            {
                this.errorLable.Content = "price must be an integer";
            }
        }

        private async Task addNewDish(Dish dish, FileInfo img = null)
        {
            WebClient<Dish> client = new WebClient<Dish>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/AddNewDish"
            };

            bool result;
            if (img == null)
            {
                this.errorLable.Content = "to create a dish image is requayerd";
                return;
            }
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
                this.errorLable.Content = "create operation failed";
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
