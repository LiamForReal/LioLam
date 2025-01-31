using Microsoft.AspNetCore.Mvc;
using LiolamResteurent;
using WebApiClient;
using NuGet.Protocol;
namespace RestaurantWebApp.Controllers
{
    public class CustomerController : Controller
    {
        [HttpPost]
        public IActionResult LogIn(string userName, string password)
        {
            WebClient<string> client = new WebClient<string>();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Customer/GetLogIn";
            client.AddParameter("userName", userName);
            client.AddParameter("password", password);
            try
            {
                string customerCheck = client.Get().Result;
                if (customerCheck == null)
                {
                    //return someting 
                    ViewBag.Error = true;
                    return View("ShowLogInForm");
                }
                ViewBag.Error = false;
                HttpContext.Session.SetString("Id", customerCheck);//session is the thread the server allocate to client to handle in my project it is a stateless space
                                                                   //the id property is added to the setion
                                                                   //ViewBag.Id = HttpContext.Session.GetString(customerCheck);
                return RedirectToAction("Method", "controller");
            }
            catch (Exception ex)
            {
                Console.WriteLine("data base is open and the code cannot access it: " + ex.Message);
                ViewBag.Error = true;
                return View("ShowLogInForm");
            }
            
        }

        [HttpGet]
        public IActionResult ShowLogInForm()
        {
            return View();
        }

        [HttpPost]

        public IActionResult SignUp(string customerId, string CustomerUserName, int CustomerHouse, int CityId, int StreetId, string CustomerPhone, string CustomerMail, string CustomerPassword, string CustomerImage)
        {
            WebClient<Customers> client = new WebClient<Customers>();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Customer/SignUp";

            Customers customer = new Customers(customerId, CustomerUserName, CustomerHouse, CityId, StreetId, CustomerPhone, CustomerMail, CustomerPassword, CustomerImage);
            if (client.Post(customer).Result == false)
            {
                //return someting 
                ViewBag.Error = true;
                return View("ShowSignUpForm");
            }
            HttpContext.Session.SetString("Id", customer.Id);//session is the thread the server allocate to client to handle in my project it is a stateless space
            //the id property is added to the setion
            //ViewBag.Id = HttpContext.Session.GetString(customerCheck);
            return RedirectToAction("Method", "controller");
        }

        [HttpGet]
        public IActionResult ShowSignUpForm()
        {
            //city and strits lists from ws
            WebClient<registerViewModel> client = new WebClient<registerViewModel>();
            registerViewModel registerViewModel = new registerViewModel();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Customer/ShowSignUp";
            registerViewModel = client.Get().Result;
            return View(registerViewModel);
        }
    }
}
