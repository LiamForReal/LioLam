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
    static ChefsPage ChefsPage;
    static CustomersPage customersPage;
    static OrdersPage ordersPage;
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
        {
            loginPage = new LogInPage();
            this.LogLabel.Text = "Log In";
            
            this.LogIcon.Source = new BitmapImage(new Uri("/Icons/user.png", UriKind.Relative)); ; // Login icon
            isLogin = false;
            this.buttons.Visibility = Visibility.Hidden;
            this.data.Child = loginPage;
            loginPage.LoginSuccessful += OnLoginSuccessful;
        }
        else
        {
            this.LogLabel.Text = "Log In";
            this.LogIcon.Source = new BitmapImage(new Uri("/Icons/user.png", UriKind.Relative)); ; // Revert to login icon
            isLogin = false;
            this.buttons.Visibility = Visibility.Hidden;
            Index();
        }
    }

    private void OnLoginSuccessful(object sender, EventArgs e)
    {
        this.LogLabel.Text = "Log Out";
        
        this.LogIcon.Source = new BitmapImage(new Uri("/Icons/logout.png", UriKind.Relative)); ; // Logout icon
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

    private void ChefsButton_Click(object sender, RoutedEventArgs e)
    {
        if (ChefsPage == null)
            ChefsPage= new ChefsPage();
        this.data.Child = ChefsPage;
    }

    private void CustomersButton_Click(object sender, RoutedEventArgs e)
    {
        if (customersPage == null)
            customersPage = new CustomersPage();
        this.data.Child = customersPage;
    }

    private void ordersButton_Click(object sender, RoutedEventArgs e)
    {
        if(ordersPage == null)
            ordersPage = new OrdersPage();
        this.data.Child = ordersPage;
    }
}