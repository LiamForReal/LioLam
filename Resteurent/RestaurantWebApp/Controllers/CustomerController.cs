using Microsoft.AspNetCore.Mvc;
using LiolamResteurent;
using WebApiClient;
using NuGet.Protocol;
namespace RestaurantWebApp.Controllers
{
    public class CustomerController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> LogIn(string userName, string password)
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
                string customerCheck = await client.Get();
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
        public async Task<IActionResult> SignUp(Customers customers, IFormFile file)
        {
            WebClient<Customers> client = new WebClient<Customers>
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/SignUp"
            };


            // Read image stream
          
                // Send the request with customer data and image
                bool result = await client.Post(customers, file.OpenReadStream());

                if (!result)
                {
                    ViewBag.Error = true;
                    return View("ShowSignUpForm");
                }
      


            // Store session info for logged-in user
            HttpContext.Session.SetString("Id", customers.Id);

            // Redirect to a successful page
            return RedirectToAction("GetDefaultScreen", "Guest"); // Change "Dashboard" to your actual target page
        }


        [HttpGet]
        public async Task<IActionResult> ShowSignUpForm()
        {
            //city and strits lists from ws
            WebClient<registerViewModel> client = new WebClient<registerViewModel>();
            registerViewModel registerViewModel = new registerViewModel();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Customer/ShowSignUp";
            registerViewModel = await client.Get();
            return View(registerViewModel);
        }
    }
}
