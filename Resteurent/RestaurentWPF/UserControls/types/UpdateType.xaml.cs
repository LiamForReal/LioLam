using LiolamResteurent;
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
    /// Interaction logic for UpdateType.xaml
    /// </summary>
    public partial class UpdateType : Window
    {
        private Category loadedType;
        public UpdateType(string id)
        {
            InitializeComponent();
            this.errorLable.Content = "";
            setScreenByTypeId(id);
        }
        private async Task setScreenByTypeId(string id)
        {
            WebClient<string> client = new WebClient<string>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/GetTypeById"
            };

            client.AddParameter("id", id);
            string typeName = await client.Get();
            Category type = new Category()
            {
                Id = id,
                TypeName = typeName,
            };
            MessageBox.Show(type.TypeName);
            this.DataContext = type;

            loadedType = new Category()
            {
                Id = type.Id,
                TypeName = type.TypeName
            };
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button updateButton = sender as Button;
                string typeId = updateButton.Tag.ToString();

                Category type = new Category()
                {
                    Id = typeId,
                    TypeName = this.nameTextBox.Text
                };

                if (type.TypeName == "")
                {
                    errorLable.Content = "name cannot be empty";
                    return;
                }
                else if (loadedType == type)
                {
                    this.Close();
                }
                else updateTypeDetails(type);
            }
            catch (Exception ex)
            {
                this.errorLable.Content = "price must be an integer";
            }
        }

        private async Task updateTypeDetails(Category type)
        {
            WebClient<Category> client = new WebClient<Category>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Manager/UpdateType"
            };
            
            bool result = await client.Post(type);


            if (result == true)
            {
                this.Close();
            }
            else
            {
                this.errorLable.Content = "update operation failed";
            }
        }
    }
}
