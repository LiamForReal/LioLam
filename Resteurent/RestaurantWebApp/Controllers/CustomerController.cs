using Microsoft.AspNetCore.Mvc;
using LiolamResteurent;
using WebApiClient;
using NuGet.Protocol;
namespace RestaurantWebApp.Controllers
{
    public class CustomerController : Controller
    {
        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            WebClient<string> client = new WebClient<string>();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Customer/GetLogIn";
            client.AddParameter("userName", userName);
            client.AddParameter("password", password);
            string customerCheck = client.Get().Result;
            if(customerCheck == null)
            {
                //return someting 
                ViewBag.Error = true;
                return View("ShowLoginForm");
            }
            HttpContext.Session.SetString("Id", customerCheck);//session is the thread the server allocate to client to handle in my project it is a stateless space
            //the id property is added to the setion
            //ViewBag.Id = HttpContext.Session.GetString(customerCheck);
            return RedirectToAction("Method", "controller");
        }

        [HttpGet]
        public IActionResult ShowLoginForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(Customers customer)
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShowSignUpForm()
        {
            //city and strits lists from ws
            return View();
        }
    }
}
