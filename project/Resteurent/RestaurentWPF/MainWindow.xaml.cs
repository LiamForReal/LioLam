using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestaurantWPF.UserControls;
namespace RestaurentWPF;


/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static bool isLogin;

    static LogInPage loginPage;
    static startPage startPage;
    static DishesPage dishesPage;
    public MainWindow()
    {
        InitializeComponent();
        isLogin = false;
        setStartWindows();
        Index();
    }

    private void Index()
    {
        if(startPage == null)
        {
            startPage = new startPage();
        }
        this.data.Child = startPage;
    }
    private void setStartWindows()
    {
        if (isLogin) this.buttons.Visibility = Visibility.Visible;
        else this.buttons.Visibility = Visibility.Hidden;
    }

    private void LogInButton_Click(object sender, RoutedEventArgs e)
    {
        if (!isLogin)
            loginPage = new LogInPage();
        
        Button button = (Button)(sender);
        if (isLogin)
        {
            button.Content = "Log out";
        }
        else
        {
            button.Content = "Log In";
            this.data.Child = loginPage;
            isLogin = true;
        }
        setStartWindows();
    }

    private void DishesButton_Click(object sender, RoutedEventArgs e)
    {
        if (dishesPage == null)
            dishesPage = new DishesPage();
        this.data.Child = dishesPage;
    }
}