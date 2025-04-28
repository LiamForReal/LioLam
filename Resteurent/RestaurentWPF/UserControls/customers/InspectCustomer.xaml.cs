using LiolamResteurent;
using Models;
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
using System.Windows.Shapes;
using WebApiClient;

namespace RestaurantWindowsPF.UserControls
{
    /// <summary>
    /// Interaction logic for InspectCustomer.xaml
    /// </summary>
    public partial class InspectCustomer : Window
    {
        public InspectCustomer(string customerId)
        {
            InitializeComponent();
            getCustomerById(customerId);
        }
        private async Task getCustomerById(string id)
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
            

            this.DataContext = customer;
            this.CustomerCity.Text = customer.city.CityName;
            this.CustomerStreet.Text = customer.street.StreetName;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
