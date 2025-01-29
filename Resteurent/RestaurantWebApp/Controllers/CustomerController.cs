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
        public IActionResult SignUp(Customers customer)
        {
            WebClient<string> client = new WebClient<string>();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Customer/GetLogIn";
            client.AddParameter("customerId",customer.Id);
            client.AddParameter("CustomerUserName", customer.CustomerUserName);
            client.AddParameter("CustomerHouse", customer.CustomerHouse.ToString());
            client.AddParameter("CityId", customer.city.Id);
            client.AddParameter("StreetId", customer.street.Id);
            client.AddParameter("CustomerPhone", customer.CustomerPhone);
            client.AddParameter("CustomerMail", customer.CustomerMail);
            client.AddParameter("CustomerPassword",customer.CustomerPassword);
            client.AddParameter("CustomerImage", customer.CustomerImage);
            string customerCheck = client.Get().Result;
            if (customerCheck == null)
            {
                //return someting 
                ViewBag.Error = true;
                return View("ShowSignUpForm");
            }
            HttpContext.Session.SetString("Id", customerCheck);//session is the thread the server allocate to client to handle in my project it is a stateless space
            //the id property is added to the setion
            //ViewBag.Id = HttpContext.Session.GetString(customerCheck);
            return RedirectToAction("Method", "controller");
        }

        [HttpGet]
        public IActionResult ShowSignUpForm()
        {
            //city and strits lists from ws
            return View();
        }
    }
}
