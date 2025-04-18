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
    /// Interaction logic for UpdateChef.xaml
    /// </summary>
    public partial class UpdateChef : Window
    {
        private FileInfo readerPictureFile;
        private Chef loadedChef;
        public UpdateChef(string ChefId)
        {
            InitializeComponent();
            this.errorLable.Content = "";
            setScreenByChefId(ChefId);
        }

        private async Task<bool> IsChefExist(string firstName, string lastName)
        {
            WebClient<bool> client = new WebClient<bool>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/IsChefExist"
            };

            client.AddParameter("firstName", firstName);
            client.AddParameter("lastName", lastName);

            return await client.Get();
        }
        private async Task setScreenByChefId(string id)
        {
            WebClient<Chef> client = new WebClient<Chef>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetChefById"
            };

            client.AddParameter("id", id);
            Chef Chef = await client.Get();

            this.DataContext = Chef;


            loadedChef = new Chef()
            {
                Id = Chef.Id,
                ChefFirstName = Chef.ChefFirstName,
                ChefLastName = Chef.ChefLastName,
            };
        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button updateButton = sender as Button;
                string ChefId = updateButton.Tag.ToString();

                this.errorLable.Content = "";
                
                Chef Chef = new Chef()
                {
                    Id = ChefId,
                    ChefFirstName = this.firstNameTextBox.Text,
                    ChefLastName = this.lastNameTextBox.Text,
                };

                if (Chef.ChefFirstName == "" || Chef.ChefLastName == "")
                {
                    errorLable.Content = "first and last name cannot be empty";
                    return;
                }
                else if (loadedChef.ChefFirstName != Chef.ChefFirstName && loadedChef.ChefLastName != Chef.ChefLastName && await IsChefExist(Chef.ChefFirstName, Chef.ChefLastName))
                {
                    errorLable.Content = "Chef with that name already exist";
                    return;
                }
                else if (loadedChef == Chef && this.readerPictureFile == null)
                {
                    this.Close();
                }
                else await updateChefDetails(Chef, this.readerPictureFile);
            }
            catch (Exception ex)
            {
                this.errorLable.Content = "price must be an integer";
            }
        }

        private async Task updateChefDetails(Chef Chef, FileInfo img = null)
        {
            WebClient<Chef> client = new WebClient<Chef>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/UpdateChef"
            };

            bool result;
            if (img == null)
                result = await client.Post(Chef);
            else
            {
                Chef.ChefImage = img.Name;
                Stream imgStream = img.OpenRead();
                result = await client.Post(Chef, imgStream);
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
