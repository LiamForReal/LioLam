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
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Window
    {
        private FileInfo readerPictureFile;
        private Customer loadedCustomer;
        public UpdateCustomer(string customerId)
        {
            InitializeComponent();
            this.errorLable.Content = "";
            setScreenByCustomerId(customerId);
        }
        //SelectedValue="{Binding SelectedCityId, Mode=TwoWay}"

        private async Task<CustomerLocationView> SetScreenByCitiesAndStreets()
        {
            WebClient<CustomerLocationView> client = new WebClient<CustomerLocationView>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetUpdateCustomerView"
            };

            return await client.Get();
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
        private async Task setScreenByCustomerId(string id)
        {
            WebClient<Customer> client = new WebClient<Customer>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/GetCustomerById"
            };

            client.AddParameter("id", id);
            Customer customer = await client.Get();

            CustomerLocationView customerLocationView = await SetScreenByCitiesAndStreets();

            this.DataContext = customer;

            this.CityComboBox.ItemsSource = customerLocationView.Cities;

            foreach(City city in customerLocationView.Cities)
            {
                if(city.Id == customer.city.Id)
                {
                    this.CityComboBox.SelectedItem = city;
                    break;
                }
            }
            

            this.StreetComboBox.ItemsSource = customerLocationView.Streets;

            foreach (Street street in customerLocationView.Streets)
            {
                if (street.Id == customer.street.Id)
                {
                    this.StreetComboBox.SelectedItem = street;
                    break;
                }
            }

            loadedCustomer = new Customer()
            {
                Id = customer.Id, 
                CustomerUserName = customer.CustomerUserName,
                CustomerPassword = customer.CustomerPassword,
                CustomerHouse = customer.CustomerHouse,
                CustomerMail = customer.CustomerMail, 
                CustomerPhone = customer.CustomerPhone,
                city = customer.city, 
                street = customer.street, 
                IsOwner = true
            };
        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button updateButton = sender as Button;
                string customerId = updateButton.Tag.ToString();

                Customer customer = new Customer()//need to change customer
                {
                    Id = customerId,
                    CustomerUserName = this.CustomerUsername.Text,
                    CustomerPassword = this.CustomerPassword.Text, 
                    CustomerMail = this.CustomerMail.Text, 
                    CustomerHouse = int.Parse(this.CustomerHouse.Text),
                    CustomerPhone = this.CustomerPhone.Text,
                    city = (City)(this.CityComboBox.SelectedItem),
                    street = (Street)(this.StreetComboBox.SelectedItem),
                    IsOwner = true
                };

                if (loadedCustomer.Equals(customer) && this.readerPictureFile == null)
                {
                    this.Close();
                }
                else if (customer.CustomerUserName == "" || customer.CustomerPassword == "")
                {
                    errorLable.Content = "user name or password cannot be empty";
                    return;
                }
                else if (loadedCustomer.CustomerUserName != customer.CustomerUserName && await IsCustomerExist(customer.CustomerUserName))
                {
                    errorLable.Content = "dish with that name already exist";
                    return;
                }
                else if(customer.CustomerMail == "" || customer.CustomerPhone == "")
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
                else if (customer.CustomerPhone.Length != 10 || !int.TryParse(customer.CustomerPhone, out _) && !customer.CustomerPhone.StartsWith("05"))
                {
                    errorLable.Content = "invalid Phone, should contains 10 nameric characters";
                    return;
                }
                else await updateCustomersDetails(customer, this.readerPictureFile);
            }
            catch (Exception ex)
            {
                this.errorLable.Content = "house must be an integer";
            }
        }

        private async Task updateCustomersDetails(Customer customer, FileInfo img = null)
        {
            WebClient<Customer> client = new WebClient<Customer>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/UpdateCustomer"
            };
            bool result;
            if (img == null)
                result = await client.Post(customer);
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
