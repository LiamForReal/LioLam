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
    /// Interaction logic for AddChef.xaml
    /// </summary>
    public partial class AddChef : Window
    {
        private FileInfo readerPictureFile;
        public AddChef()
        {
            InitializeComponent();
            this.errorLable.Content = "";
        }


        private async Task<bool> IsChefExist(string firstName, string lastName)
        {
            WebClient<bool> client = new WebClient<bool>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/IsChefExist"
            };
 
            client.AddParameter("firstName", firstName);
            client.AddParameter("lastName", lastName);

            return await client.Get();
        }
        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.errorLable.Content = "";

                Chef Chef = new Chef()
                {
                    ChefFirstName = this.firstNameTextBox.Text, 
                    ChefLastName = this.lastNameTextBox.Text
                };

                if (Chef.ChefFirstName == "" || Chef.ChefLastName == "")
                {
                    errorLable.Content = "first and last name cannot be empty";
                    return;
                }
                else if (await IsChefExist(Chef.ChefFirstName, Chef.ChefLastName))
                {
                    errorLable.Content = "Chef already exsist";
                    return;
                }
                await addNewChef(Chef, this.readerPictureFile);
            }
            catch (Exception ex)
            {
                this.errorLable.Content = "price must be an integer";
            }
        }

        private async Task addNewChef(Chef Chef, FileInfo img = null)
        {
            WebClient<Chef> client = new WebClient<Chef>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/AddNewChef"
            };

            bool result;
            if (img == null)
            {
                this.errorLable.Content = "to create a chef, image is requayerd";
                return;
            }
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
