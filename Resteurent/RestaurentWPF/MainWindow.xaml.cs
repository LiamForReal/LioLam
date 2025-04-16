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
using RestaurantWindowsPF.UserControls;
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
    static TypesPage typesPage;
    public MainWindow()
    {
        InitializeComponent();
        isLogin = false;
        this.buttons.Visibility = Visibility.Hidden;
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

    private void LogInButton_Click(object sender, RoutedEventArgs e)
    {
        if (!isLogin)
            loginPage = new LogInPage();

        Button button = (Button)(sender);
        button.Content = "Log In";
        this.data.Child = loginPage;
        isLogin = false;
        this.buttons.Visibility = Visibility.Hidden;
        loginPage.LoginSuccessful += OnLoginSuccessful;
    }

    private void OnLoginSuccessful(object sender, EventArgs e)
    {
        this.LogInButton.Content = "Log out";
        isLogin = true;
        this.buttons.Visibility = Visibility.Visible;
    }

    private void DishesButton_Click(object sender, RoutedEventArgs e)
    {
        if (dishesPage == null)
            dishesPage = new DishesPage();
        this.data.Child = dishesPage;
    }

    private void TypesButton_Click(object sender, RoutedEventArgs e)
    {
        if (typesPage == null)
            typesPage = new TypesPage();
        this.data.Child = typesPage;
    }
}