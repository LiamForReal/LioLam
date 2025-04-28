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
    /// Interaction logic for TypesPage.xaml
    /// </summary>
    public partial class TypesPage : UserControl
    {
        static UpdateType updateTypePage;
        static AddType addTypePage;
        public TypesPage()
        {
            InitializeComponent();
            GetAllTypes();
        }
        private async Task GetAllTypes()
        {
            WebClient<List<Category>> client = new WebClient<List<Category>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetTypes"
            };
           
            this.listView.ItemsSource = await client.Get();
        }

        private async void updateType_Click(object sender, RoutedEventArgs e)
        {
            Button updateButton = sender as Button;
            string typeId = updateButton.Tag.ToString();
            updateTypePage = new UpdateType(typeId);
            updateTypePage.ShowDialog();
            await GetAllTypes();
        }

        private async void deleteType_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            string typeId = deleteButton.Tag.ToString();
            string typeName = await getTypeNameById(typeId);
            string messageBoxText = $"Confirm delete: '{typeName}'?";
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                await deleteType(typeId);
                await GetAllTypes();
            }
        }
        private async Task deleteType(string typeId)
        {
            WebClient<string> client = new WebClient<string>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/DeleteType"
            };

            bool result = await client.Post(typeId);

            if (!result)
            {
                //error
            }
        }
        private async Task<string> getTypeNameById(string id)
        {
            WebClient<Category> client = new WebClient<Category>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetTypeById"
            };

            client.AddParameter("id", id);
            Category type = await client.Get();
            return type.TypeName;
        }
        private async void addNewType_Click(object sender, RoutedEventArgs e)
        {
            addTypePage = new AddType();
            addTypePage.ShowDialog();
            await GetAllTypes();
        }
    }
}
