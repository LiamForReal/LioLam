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

namespace RestaurantWindowsPF.UserControls.dishes
{
    /// <summary>
    /// Interaction logic for updateDish.xaml
    /// </summary>
    public partial class updateDish : Window
    {
        public updateDish(string DishId)
        {
            InitializeComponent();
            setDishById(DishId);
        }

        private async Task setDishById(string id)
        {
            return;
        }
    }
}
