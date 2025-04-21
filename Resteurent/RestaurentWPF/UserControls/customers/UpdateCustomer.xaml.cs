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

        private async Task<RegisterViewModel> SetScreenByCitiesAndStreets()
        {
            WebClient<RegisterViewModel> client = new WebClient<RegisterViewModel>()
            {
                Scheme = "http",
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
                Scheme = "http",
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
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/GetCustomerById"
            };

            client.AddParameter("id", id);
            Customer customer = await client.Get();

            RegisterViewModel registerViewModel = await SetScreenByCitiesAndStreets();

            this.DataContext = customer;

            this.CityComboBox.ItemsSource = registerViewModel.Cities;
            this.CityComboBox.SelectedValue = customer.city;

            this.StreetComboBox.ItemsSource = registerViewModel.Streets;
            this.CityComboBox.SelectedValue = customer.street;

            loadedCustomer = new Customer()
            {
                Id = customer.Id, 
                CustomerUserName = customer.CustomerUserName,
                CustomerPassword = customer.CustomerPassword,
                CustomerHouse = customer.CustomerHouse,
                CustomerMail = customer.CustomerMail, 
                CustomerPhone = customer.CustomerPhone
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
                    CustomerMail = this.CustomerEmail.Text, 
                    CustomerHouse = int.Parse(this.CustomerHouse.Text),
                    CustomerPhone = this.CustomerPhone.Text,
                };
                //checks
                //if (dish.DishName == "" || dish.DishDescription == "")
                //{
                //    errorLable.Content = "name or description cannot be empty";
                //    return;
                //}
                //else if (dish.DishPrice <= 0)
                //{
                //    errorLable.Content = "price cannot be negative or 0";
                //    return;
                //}
                //else if (loadedDish.DishName != dish.DishName && await IsDishExist(dish.DishName))
                //{
                //    errorLable.Content = "dish with that name already exist";
                //    return;
                //}
                //else if (dish.types.Count < 1 || dish.chefs.Count < 1)
                //{
                //    errorLable.Content = "every dish must contains at list one chef and type";
                //    return;
                //}
                //else if (loadedDish == dish && this.readerPictureFile == null)
                //{
                //    this.Close();
                //}
                //else
                await updateCustomersDetails(customer, this.readerPictureFile);
            }
            catch (Exception ex)
            {
                this.errorLable.Content = "price must be an integer";
            }
        }

        private async Task updateCustomersDetails(Customer customer, FileInfo img = null)
        {
            WebClient<Customer> client = new WebClient<Customer>()
            {
                Scheme = "http",
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
