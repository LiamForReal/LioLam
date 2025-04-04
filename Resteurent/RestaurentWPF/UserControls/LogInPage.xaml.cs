using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using LiolamResteurent;
using WebApiClient;

namespace RestaurantWPF.UserControls
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogInPage : UserControl
    {
        Customers customer;
        public Customers getCustomer
        {
            get
            {
                return this.customer;
            }
        }
        public LogInPage()
        {
            InitializeComponent();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            this.userNameInput.Text = "";
            this.passwordInput.Text = "";
        }

        private async void submit_Click(object sender, RoutedEventArgs e)
        {
            WebClient<Customers> client = new WebClient<Customers>();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/manager/IsAdmin";

            //Console.WriteLine(client.buildURI());
            Customers customer = new Customers();
            customer.CustomerUserName = this.userName.ToString();
            customer.CustomerPassword = this.password.ToString();
            this.errorLable.Visibility = Visibility.Hidden;
            try
            {
                string adminId = await client.Post(customer);
                if (adminId == "")
                {
                    this.errorLable.Visibility = Visibility.Visible;
                    return;
                }
                Console.WriteLine("admin is in!");
                customer.Id = adminId;
                //show main window


            }
            catch (Exception ex)
            {
                Console.WriteLine("data base is open and the code cannot access it: " + ex.Message);
            }
        }
    }
}
