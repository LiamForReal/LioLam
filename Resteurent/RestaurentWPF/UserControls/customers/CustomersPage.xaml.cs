using LiolamResteurent;
using RestaurantWPF.UserControls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebApiClient;

namespace RestaurantWindowsPF.UserControls
{
    /// <summary>
    /// Interaction logic for CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : UserControl
    {
        static InspectCustomer inspactCustomerPage;
        static UpdateCustomer updateCustomerPage;
        static AddCustomer addCustomerPage;
        public CustomersPage()
        {
            InitializeComponent();
            GetAllCustomers();
        }
        private async Task GetAllCustomers()
        {
            WebClient<List<Customer>> client = new WebClient<List<Customer>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetCustomers"
            };

            this.listView.ItemsSource = await client.Get();
        }

        private void inspectCustomer_Click(object sender, RoutedEventArgs e)
        {
            Button inspactButton = sender as Button;
            string CustomerId = inspactButton.Tag.ToString();
            inspactCustomerPage = new InspectCustomer(CustomerId);
            inspactCustomerPage.ShowDialog();
        }

        private async void updateCustomer_Click(object sender, RoutedEventArgs e)
        {
            Button updateButton = sender as Button;
            string CustomerId = updateButton.Tag.ToString();
            updateCustomerPage = new UpdateCustomer(CustomerId);
            updateCustomerPage.ShowDialog();
            await GetAllCustomers();
        }

        private async void deleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            string CustomerId = deleteButton.Tag.ToString();
            string CustomerName = await getCustomerNameById(CustomerId);
            string messageBoxText = $"Confirm delete: '{CustomerName}'?";
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                await deleteCustomer(CustomerId);
                await GetAllCustomers();
            }
        }
        private async Task deleteCustomer(string CustomerId)
        {
            WebClient<string> client = new WebClient<string>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/DeleteCustomer"
            };

            bool result = await client.Post(CustomerId);

            if (!result)
            {
                //error
            }
        }
        private async Task<string> getCustomerNameById(string id)
        {
            WebClient<Customer> client = new WebClient<Customer>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/GetCustomerbyId"
            };

            client.AddParameter("id", id);
            Customer Customer = await client.Get();
            return Customer.CustomerUserName;
        }
        private async void addNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            addCustomerPage = new AddCustomer();
            addCustomerPage.ShowDialog();
            await GetAllCustomers();
        }
    }
}
