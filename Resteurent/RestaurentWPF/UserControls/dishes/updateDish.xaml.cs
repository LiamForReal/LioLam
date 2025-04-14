using LiolamResteurent;
using Microsoft.Win32;
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
using System.Xml;
using WebApiClient;

namespace RestaurantWindowsPF.UserControls
{
    /// <summary>
    /// Interaction logic for updateDish.xaml
    /// </summary>
    public partial class updateDish : Window
    {
        private FileInfo readerPictureFile;
        private string dBimage;
        public updateDish(string DishId)
        {
            InitializeComponent();
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

            dBimage = System.IO.Path.GetFileName(dish.DishImage);
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dish dish = new Dish()
                {
                    DishName = this.nameTextBox.Text,
                    DishDescription = this.descriptionTextBox.Text,
                    DishPrice = int.Parse(this.priceTextBox.Text)
                };
                if(this.readerPictureFile != null)
                {
                    Stream stream = this.readerPictureFile.OpenRead();
                    updateDishDetails(dish, stream);
                }
                updateDishDetails(dish);
            }
            catch(Exception ex)
            {
                this.priceTextBox.Text = "";
            }
        }

        private async Task updateDishDetails(Dish dish, Stream img = null)
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
            else result = await client.Post(dish, img);

            if (result == true)
            {
                //good behavior
            }
            else
            {
                //bad behavior
            }
        }

        private void picktureInput_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            // Specify the types of images which can be picked
            ofd.Filter = "Image files (*.png;*.jpeg;*.jpg;*.webp;*.jiff)|*.png;*.jpeg;*.jpg;*.webp;*.jiff";
            if (ofd.ShowDialog() == true)
            {
                string selectedFilePath = ofd.FileName;
                string selectedFileName = System.IO.Path.GetFileName(selectedFilePath);

                if (string.Equals(dBimage, selectedFileName, StringComparison.OrdinalIgnoreCase))
                {
                    this.readerPictureFile = null;
                    return;
                }
                this.readerPictureFile = new FileInfo(ofd.FileName);
                this.imgRenderer.Source = new BitmapImage(new Uri(this.readerPictureFile.FullName));
            }
        }
    }
}
