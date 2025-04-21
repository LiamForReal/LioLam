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
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetCustomerById"
            };

            client.AddParameter("id", id);
            Customer customer = await client.Get();
            
            WebClient<CustomerLocation> client2 = new WebClient<CustomerLocation>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetCustomerLocationById"
            };

            client2.AddParameter("id", id);
            CustomerLocation CustomerLocation = await client2.Get();

            this.DataContext = customer;
            this.CustomerCity.Text = CustomerLocation.city.CityName;
            this.CustomerStreet.Text = CustomerLocation.street.StreetName;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
