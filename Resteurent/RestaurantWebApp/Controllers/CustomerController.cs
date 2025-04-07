using Microsoft.AspNetCore.Mvc;
using LiolamResteurent;
using WebApiClient;
using NuGet.Protocol;
using System.Runtime.CompilerServices;
using NuGet.Packaging.Signing;

namespace RestaurantWebApp.Controllers
{
    public class CustomerController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> LogIn(string userName, string password)
        {

            WebClient<string> client = new WebClient<string>
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/LogIn"
            };
            client.AddParameter("userName", userName);
            client.AddParameter("password", password);
            
            try
            {
                string customerId = await client.Get();
                if (customerId == "")
                {
                    //return someting 
                    ViewBag.Error = true;
                    return View("ShowLogInForm");
                }
                ViewBag.Error = false;
                TempData["Id"] = customerId; //actual edit tmp data

                HttpContext.Session.SetString("Id", customerId);//session is the thread the server allocate to client to handle in my project it is a stateless space
                                                                   //the id property is added to the setion
                                                                   //ViewBag.Id = HttpContext.Session.GetString(customerCheck);
                return RedirectToAction("GetDefaultScreen", "Customer");
            }
            catch (Exception ex)
            {
                Console.WriteLine("data base is open and the code cannot access it: " + ex.Message);
                ViewBag.Error = true;
                return View("ShowLogInForm");
            }

        }


        [HttpGet]
        public async Task<IActionResult> GetDefaultScreen()
        {
            WebClient<welcomeDetails> client = new WebClient<welcomeDetails>
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/GetWelcomeDetails"
            };

            if(TempData["Id"] != null)
            {
                client.AddParameter("id", TempData["Id"].ToString());
                welcomeDetails welcomeDetails = await client.Get();
                return View("GetDefaultScreen", welcomeDetails);
            }
            return View("GetDefaultScreen");
        }

        [HttpGet]
        public IActionResult ShowLogInForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Customers customers, IFormFile Image)
        {
            Console.WriteLine($"customer id is {customers.Id}" );
            WebClient<Customers> client = new WebClient<Customers>
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/SignUp"
            };
            customers.CustomerImage = Image.FileName;

            // Read image stream

            // Send the request with customer data and image
            bool result = await client.Post(customers, Image.OpenReadStream());

            if (!result)
            {
                ViewBag.Error = true;
                return RedirectToAction("ShowSignUpForm", "Customer");
            }


            TempData["Id"] = customers.Id; //actual edit tmp data
            // Store session info for logged-in user
            HttpContext.Session.SetString("Id", customers.Id);

            // Redirect to a successful page
            return RedirectToAction("GetDefaultScreen", "Customer"); // Change "Dashboard" to your actual target page
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(Customers customers, IFormFile Image)
        {
            Console.WriteLine($"customer id is {customers.Id}");
            WebClient<Customers> client = new WebClient<Customers>
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/UpdateExistingUser"
            };
            customers.CustomerImage = Image.FileName;

            // Read image stream

            // Send the request with customer data and image
            bool result = await client.Post(customers, Image.OpenReadStream());

            if (!result)
            {
                ViewBag.Error = true;
                return RedirectToAction("ShowEditSignUpForm", "Customer");
            }
            // Redirect to a successful page
            return RedirectToAction("GetDefaultScreen", "Customer"); // Change "Dashboard" to your actual target page
        }

        [HttpGet]
        public async Task<IActionResult> ShowSignUpForm()
        {
            //city and strits lists from ws
            WebClient<registerViewModel> client = new WebClient<registerViewModel>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/ShowSignUp"
            };
            registerViewModel registerViewModel = await client.Get();

            Account account = new Account()
            {
                Customer = null,
                registerView = registerViewModel
            };

            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> ShowEditSignUpForm()
        {
            
            //city and strits lists from ws
            WebClient<registerViewModel> client = new WebClient<registerViewModel>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/ShowSignUp"
            };
            registerViewModel registerViewModel = await client.Get();

            WebClient<Customers> client2 = new WebClient<Customers>()
            {
                Scheme = "http",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/GetCustomerById"
            };

            Console.WriteLine("this is tmp data: " + TempData["Id"]); //tmp data is null some how
            client2.AddParameter("id", TempData["Id"].ToString()); //check it later!!!
            Customers customer = await client2.Get();

            Account account = new Account()
            {
                Customer = customer, 
                registerView = registerViewModel
            };

            return View("ShowSignUpForm", account);
        }

    }
}
