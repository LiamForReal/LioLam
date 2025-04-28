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
    /// Interaction logic for ChefsPage.xaml
    /// </summary>
    public partial class ChefsPage : UserControl
    {
        static UpdateChef updateChefPage;
        static AddChef addChefPage;
        public ChefsPage()
        {
            InitializeComponent();
            GetAllChefs();
        }


        private async Task GetAllChefs()
        {
            WebClient<List<Chef>> client = new WebClient<List<Chef>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetChefs"
            };

            this.listView.ItemsSource = await client.Get();
        }

        private async void updateChef_Click(object sender, RoutedEventArgs e)
        {
            Button updateButton = sender as Button;
            string ChefId = updateButton.Tag.ToString();
            updateChefPage = new UpdateChef(ChefId);
            updateChefPage.ShowDialog();
            await GetAllChefs();
        }

        private async void deleteChef_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            string ChefId = deleteButton.Tag.ToString();
            string ChefName = await getChefNameById(ChefId);
            string messageBoxText = $"Confirm delete: '{ChefName}'?";
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                await deleteChef(ChefId);
                await GetAllChefs();
            }
        }
        private async Task deleteChef(string ChefId)
        {
            WebClient<string> client = new WebClient<string>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/DeleteChef"
            };

            bool result = await client.Post(ChefId);

            if (!result)
            {
                //error
            }
        }
        private async Task<string> getChefNameById(string id)
        {
            WebClient<Chef> client = new WebClient<Chef>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetChefById"
            };

            client.AddParameter("id", id);
            Chef Chef = await client.Get();
            return $"{Chef.ChefFirstName} {Chef.ChefLastName}";
        }
        private async void addNewChef_Click(object sender, RoutedEventArgs e)
        {
            addChefPage = new AddChef();
            addChefPage.ShowDialog();
            await GetAllChefs();
        }
    }
}
