using LiolamResteurent;
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
    /// Interaction logic for AddType.xaml
    /// </summary>
    public partial class AddType : Window
    {
        public AddType()
        {
            InitializeComponent();
        }

        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.errorLable.Content = "";

                Category type = new Category()
                { 
                    TypeName = this.nameTextBox.Text
                };

                if (type.TypeName == "")
                {
                    errorLable.Content = "type name cannot be empty";
                    return;
                }
                if(await IsTypeExist(type.TypeName))
                {
                    errorLable.Content = "type already exsist";
                    return;
                }
                await addNewType(type);
            }
            catch (Exception ex)
            {
                this.errorLable.Content = "price must be an integer";
            }
        }

        private async Task<bool> IsTypeExist(string name)
        {
            WebClient<bool> client = new WebClient<bool>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/IsTypeExist"
            };

            client.AddParameter("typeName", name);

            return await client.Get();
        }
        private async Task addNewType(Category type)
        {
            WebClient<Category> client = new WebClient<Category>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/AddNewType"
            };

            bool result = await client.Post(type);

            if (result == true)
            {
                this.Close();
            }
            else
            {
                this.errorLable.Content = "create operation failed";
            }
        }

    }
}
