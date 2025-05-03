using Microsoft.AspNetCore.Mvc;
using LiolamResteurent;
using WebApiClient;
using NuGet.Protocol;
using System.Runtime.CompilerServices;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Http;
using RestaurantWebApplication.externals;
using Models;
using System.Collections.Generic;

namespace RestaurantWebApp.Controllers
{
    public class CustomerController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> LogIn(string userName, string password)
        {

            WebClient<string> client = new WebClient<string>
            {
                Scheme = "https",
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
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("orderId");
            HttpContext.Session.Remove("productList");
            HttpContext.Session.Remove("prevOrders");
            return RedirectToAction("GetDefaultScreen");
        }
        [HttpGet]
        public async Task<IActionResult> GetDefaultScreen()
        {
            WebClient<WelcomeDetails> client = new WebClient<WelcomeDetails>
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/GetWelcomeDetails"
            };

            if(HttpContext.Session.GetString("Id") != null)
            {
                client.AddParameter("id", HttpContext.Session.GetString("Id"));
                WelcomeDetails welcomeDetails = await client.Get();
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
        public async Task<IActionResult> SignUp(Customer customers, IFormFile Image)
        {
            Console.WriteLine($"customer id is {customers.Id}" );
            WebClient<Customer> client = new WebClient<Customer>
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/SignUp"
            };

            customers.CustomerImage = Image.FileName;

            // Read image stream

            // Send the request with customer data and image
            bool result = await client.Post(customers, Image.OpenReadStream());
            ViewBag.Error = true;
            if (!result)
            {
                ViewBag.Error = true;
                return RedirectToAction("ShowSignUpForm", "Customer");
            }

            HttpContext.Session.SetString("Id", customers.Id);

            // Redirect to a successful page
            return RedirectToAction("GetDefaultScreen", "Customer"); // Change "Dashboard" to your actual target page
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(Customer customers, IFormFile Image)
        {
            Console.WriteLine($"customer id is {customers.Id}");
            WebClient<Customer> client = new WebClient<Customer>
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/UpdateExistingUser"
            };

            // Read image stream
            bool result;
            // Send the request with customer data and image
            if (Image == null || Image.Length == 0)
            {
                result = await client.Post(customers);
            }
            else
            {
                customers.CustomerImage = Image.FileName;
                result = await client.Post(customers, Image.OpenReadStream());
            }
           

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
            WebClient<CustomerLocationView> client = new WebClient<CustomerLocationView>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/ShowSignUp"
            };
            CustomerLocationView customerLocationView = await client.Get();

            Account account = new Account()
            {
                Customer = null,
                registerView = customerLocationView
            };

            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> ShowEditSignUpForm()
        {
            
            //city and strits lists from ws
            WebClient<CustomerLocationView> client = new WebClient<CustomerLocationView>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/ShowSignUp"
            };
            CustomerLocationView customerLocationView = await client.Get();

            WebClient<Customer> client2 = new WebClient<Customer>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/GetCustomerById"
            };

            client2.AddParameter("id", HttpContext.Session.GetString("Id")); //check it later!!!
            Customer customer = await client2.Get();

            Account account = new Account()
            {
                Customer = customer, 
                registerView = customerLocationView
            };

            return View("ShowSignUpForm", account);
        }

        [HttpGet]
        public async Task GetCustomerOrders(string Id)
        {
            WebClient<List<string>> client = new WebClient<List<string>>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/customer/getOrderList"
            };

            client.AddParameter("Id", Id);

            HttpContext.Session.SetObject<List<string>>("prevOrders", await client.Get());
        }

        [HttpGet]
        public async Task<IActionResult> LoadOrderById(string orderId)
        {
            WebClient<List<OrderProduct>> client = new WebClient<List<OrderProduct>> ()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/customer/LoadOrderById"
            };
            client.AddParameter("Id", HttpContext.Session.GetString("Id"));
            client.AddParameter("orderId", orderId);

            HttpContext.Session.SetObject<List<OrderProduct>>("productList", await client.Get());

            return RedirectToAction("ShowOrderScreen", "customer");
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult UpdateDishInOrder([FromBody] DishQuantityUpdateRequest request)
        {
            List<OrderProduct> products = HttpContext.Session.GetObject<List<OrderProduct>>("productList");
            foreach (var Iproduct in products)
            {
                if (Iproduct.Id == request.DishId.ToString())
                {
                    int priceForOne = Iproduct.totalPrice / Iproduct.Quatity;
                    Iproduct.Quatity = request.Quantity;
                    Iproduct.totalPrice = priceForOne * Iproduct.Quatity;
                    HttpContext.Session.SetObject("productList", products);
                    // RETURN JSON here:
                    return Ok(new { newTotalPrice = Iproduct.totalPrice, newQuantity = Iproduct.Quatity });
                }
            }
            return BadRequest(); // If product not found
        }
        [HttpGet]
        public async Task<IActionResult> AddNewDishToOrder(string dishId)
        {
            WebClient<Dish> client = new WebClient<Dish>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/guest/GetSingleDish"
            };

            client.AddParameter("id", dishId);
            Dish dish = await client.Get();

            OrderProduct product = new OrderProduct()
            {
                Image = dish.DishImage,
                Id = dishId,
                Name = dish.DishName,
                Quatity = 1,
                totalPrice = dish.DishPrice
            };
            
            if (HttpContext.Session.GetObject<List<OrderProduct>>("productList") == null)
            {
                List<OrderProduct> products = new List<OrderProduct>();
                products.Add(product);
                HttpContext.Session.SetObject<List<OrderProduct>>("productList", products);
            }
            else
            {
                bool alreadyExsist = false;
                List<OrderProduct> products = HttpContext.Session.GetObject<List<OrderProduct>>("productList");
                foreach(var Iproduct in products)
                {
                    if(Iproduct.Id == product.Id)
                    {
                        Iproduct.Quatity++;
                        alreadyExsist = true;
                    }
                }
                if(!alreadyExsist)
                    products.Add(product);
                HttpContext.Session.SetObject<List<OrderProduct>>("productList", products);
            }
            
            return RedirectToAction("ShowOrderScreen", "customer");
        }

        [HttpGet]

        public async Task<IActionResult> ShowOrderMenu()
        {
            try
            {
                
                WebClient<Menu> client = new WebClient<Menu>()
                {
                    Scheme = "https",
                    Port = 5125,
                    Host = "localhost",
                    Path = "api/Guest/GetMenu"
                };

                Menu menu = await client.Get();
                return View("GetMenu", menu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return View("GetMenu");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveDishFromOrder(string dishId)
        {
            List<OrderProduct> products = HttpContext.Session.GetObject<List<OrderProduct>>("productList");
            foreach (var Iproduct in products)
            {
                if (Iproduct.Id == dishId)
                {
                    products.Remove(Iproduct);
                    break;
                }
            }
            if(products.Count == 0)
                HttpContext.Session.SetObject<List<OrderProduct>>("productList", null);
            else HttpContext.Session.SetObject<List<OrderProduct>>("productList", products);
            
            return RedirectToAction("ShowOrderScreen", "customer");
        }

        [HttpGet]
        public async Task<IActionResult> ShowOrderScreen()
        {
            Order order = new Order();
            if (HttpContext.Session.GetString("orderId") != null)
                order.Id = HttpContext.Session.GetString("orderId");
            if (HttpContext.Session.GetObject<List<OrderProduct>>("productList") != null)
                order.products = HttpContext.Session.GetObject<List<OrderProduct>>("productList");
            await GetCustomerOrders(HttpContext.Session.GetString("Id"));
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> ShowOrderDefaultScreen()
        {
            //get Order ID
            WebClient<Order> client = new WebClient<Order>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/getCurrentOrderId"
            };
            client.AddParameter("customerId", HttpContext.Session.GetString("Id"));

            Order order = await client.Get();
            //save order Id
            HttpContext.Session.SetString("orderId", order.Id);

            await GetCustomerOrders(HttpContext.Session.GetString("Id"));

            if (HttpContext.Session.GetObject<List<OrderProduct>>("productList") != null)
                order.products = HttpContext.Session.GetObject<List<OrderProduct>>("productList");

            return View("ShowOrderScreen", order);
        }

        [HttpGet]
        public async Task<IActionResult> AddNewOrder()
        {
            Order order = new Order()
            {
                Id = HttpContext.Session.GetString("orderId"),
                CustomerId = HttpContext.Session.GetString("Id"),
                OrderDate = DateTime.Today,
                products = HttpContext.Session.GetObject<List<OrderProduct>>("productList")
            };

            WebClient<Order> client = new WebClient<Order>()
            {
                Scheme = "https",
                Port = 5125,
                Host = "localhost",
                Path = "api/Customer/AddNewOrder"
            };

            bool result = await client.Post(order);

            if (result == true)
            {
                ViewBag.Success = true;
                HttpContext.Session.SetObject<List<OrderProduct>>("productList", null);
            }
            else ViewBag.Error = true;

            return View("ShowCreditCardInfo");
        }

        [HttpGet]
        public async Task<IActionResult> ShowCreditCardInfo()
        {
            List<OrderProduct> products = HttpContext.Session.GetObject<List<OrderProduct>>("productList");
            if (products == null || products.Count == 0)
            {
                ViewBag.Error = true;
                return View("ShowOrderScreen");
            }
            return View(products.Sum(item => item.totalPrice));
        }

        [HttpPost]
        public async Task<IActionResult> PayWithCardDetails()
        {
            //no need to take the info bc it only to simulate paying 
            return RedirectToAction("AddNewOrder", "Customer");
        }
    }
}
