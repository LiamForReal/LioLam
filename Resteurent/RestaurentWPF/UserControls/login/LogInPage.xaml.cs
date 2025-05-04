using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
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

        public event EventHandler LoginSuccessful;

        public LogInPage()
        {
            InitializeComponent();
            this.userNameInput.Text = "manager";//for debuge
            this.passwordInput.Password = "tmp1";
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            this.userNameInput.Text = "";
            this.passwordInput.Password = "";
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
          
            this.statusLable.Visibility = Visibility.Hidden;

            if (this.userNameInput.Text == "" || this.passwordInput.Password == "")
            {
                this.statusLable.Content = "user name or password cannot be empty!";
                this.statusLable.Foreground = new SolidColorBrush(Colors.Red);
                this.statusLable.Visibility = Visibility.Visible;
                return;
            }
            //Console.WriteLine(client.buildURI());
            LogIn();
        }

        private async Task LogIn()
        {
            WebClient<Customer> client = new WebClient<Customer>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/IsAdmin"
            };

            Customer customer = new Customer();
            customer.CustomerUserName = this.userNameInput.Text;
            customer.CustomerPassword = this.passwordInput.Password;

            try
            {
                bool result = await client.Post(customer);
                if (!result)
                {
                    this.statusLable.Content = "wrong customer details!";
                    this.statusLable.Visibility = Visibility.Visible;
                    return;
                }
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
                this.userNameInput.Text = "";
                this.passwordInput.Password = "";
                //show main window
            }
            catch (Exception ex)
            {
                Console.WriteLine("data base is open and the code cannot access it: " + ex.Message);
                this.statusLable.Content = "an expected error!";
                this.statusLable.Visibility = Visibility.Visible;
            }
        }
    }
}
