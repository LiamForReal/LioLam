using LiolamResteurent;
using Microsoft.Win32;
using Models;
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

        private async Task<AddDishView> SetScreenByChefsAndTypes()
        {
            WebClient<AddDishView> client = new WebClient<AddDishView>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetAddDishView"
            };

            AddDishView addDishView = await client.Get();

            return addDishView;
        }

        private async Task<bool> IsDishExist(string name)
        {
            WebClient<bool> client = new WebClient<bool>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/IsDishExist"
            };

            client.AddParameter("dishName", name);

            return await client.Get();
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
            AddDishView addDishView = await SetScreenByChefsAndTypes();
            this.chefs.ItemsSource = addDishView.chefs;
            this.types.ItemsSource = addDishView.types;

            this.DataContext = dish;

            this.chefs.SelectedItems.Clear();
            this.types.SelectedItems.Clear();

            foreach (var chefInDish in dish.chefs)
            {
                var match = addDishView.chefs.FirstOrDefault(c => c.Id == chefInDish.Id);
                if (match != null)
                    this.chefs.SelectedItems.Add(match);
            }

            foreach (var typeInDish in dish.types)
            {
                var match = addDishView.types.FirstOrDefault(t => t.Id == typeInDish.Id);
                if (match != null)
                    this.types.SelectedItems.Add(match);
            }

            this.priceTextBox.Text = $"{dish.DishPrice}₪";

            loadedDish = new Dish()
            {
                Id = dish.Id,
                DishName = dish.DishName,
                DishDescription = dish.DishDescription,
                DishPrice = dish.DishPrice,
                types = dish.types, 
                chefs = dish.chefs
            };
        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
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
                    DishPrice = int.Parse(price),
                    types = this.types.SelectedItems.Cast<Category>().ToList(),
                    chefs = this.chefs.SelectedItems.Cast<Chef>().ToList()
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
                else if(loadedDish.DishName != dish.DishName && await IsDishExist(dish.DishName))
                {
                    errorLable.Content = "dish with that name already exist";
                    return;
                }
                else if (dish.types.Count < 1 || dish.chefs.Count < 1)
                {
                    errorLable.Content = "every dish must contains at list one chef and type";
                    return;
                }
                else if (loadedDish.Equals(dish) && this.readerPictureFile == null)
                {
                    this.Close();
                }
                else await updateDishDetails(dish, this.readerPictureFile);
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
