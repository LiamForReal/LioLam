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
using WebApiClient;

namespace RestaurantWindowsPF.UserControls
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        private FileInfo readerPictureFile;

        public AddCustomer()
        {
            InitializeComponent();
            this.errorLable.Content = "";
            SetScreenByCitiesAndStreets();
        }

        private async Task SetScreenByCitiesAndStreets()
        {
            WebClient<CustomerLocationView> client = new WebClient<CustomerLocationView>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetUpdateCustomerView"
            };

            CustomerLocationView customerLocationView = await client.Get();
            this.CityComboBox.ItemsSource = customerLocationView.Cities;
            this.StreetComboBox.ItemsSource = customerLocationView.Streets;
        }
        private async Task<bool> IsCustomerExist(string name)
        {
            WebClient<bool> client = new WebClient<bool>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/IsCustomerExist"
            };

            client.AddParameter("userName", name);

            return await client.Get();
        }
        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.errorLable.Content = "";

                Customer customer = new Customer()//need to change customer
                {
                    Id = this.Id.Text,
                    CustomerUserName = this.CustomerUsername.Text,
                    CustomerPassword = this.CustomerPassword.Text,
                    CustomerMail = this.CustomerMail.Text,
                    CustomerHouse = int.Parse(this.CustomerHouse.Text),
                    CustomerPhone = this.CustomerPhone.Text,
                    city = (City)(this.CityComboBox.SelectedItem),
                    street = (Street)(this.StreetComboBox.SelectedItem),
                    IsOwner = true
                };

                if (customer.CustomerUserName == "" || customer.CustomerPassword == "")
                {
                    errorLable.Content = "user name or password cannot be empty";
                    return;
                }
                else if (await IsCustomerExist(customer.CustomerUserName))
                {
                    errorLable.Content = "dish with that name already exist";
                    return;
                }
                else if (customer.CustomerMail == "" || customer.CustomerPhone == "")
                {
                    errorLable.Content = "phone and mail cannot be empty";
                    return;
                }
                else if (customer.street == null || customer.city == null)
                {
                    errorLable.Content = "city and type cannot be empty";
                    return;
                }
                else if (customer.Id.Length != 9 || !int.TryParse(customer.Id, out _))
                {
                    errorLable.Content = "invalid Id, should contains 9 nameric characters";
                    return;
                }
                else if(customer.CustomerPhone.Length != 10|| !int.TryParse(customer.CustomerPhone, out _) && !customer.CustomerPhone.StartsWith("05"))
                {
                    errorLable.Content = "invalid Phone, should contains 10 nameric characters";
                    return;
                }
                await addNewCustomer(customer, this.readerPictureFile);
            }
            catch (Exception ex)
            {
                this.errorLable.Content = "price must be an integer";
            }
        }

        private async Task addNewCustomer(Customer customer, FileInfo img = null)
        {
            WebClient<Customer> client = new WebClient<Customer>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/AddNewCustomer"
            };

            bool result;
            if (img == null)
            {
                this.errorLable.Content = "to create a customer, image is requayerd";
                return;
            }
            else
            {
                customer.CustomerImage = img.Name;
                Stream imgStream = img.OpenRead();
                result = await client.Post(customer, imgStream);
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
