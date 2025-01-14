using Microsoft.AspNetCore.Mvc;
using LiolamResteurent;
using WebApiClient;
using System.Runtime.CompilerServices;

namespace ResteurantWebApp.Controllers
{
    public class GuestController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultScreen()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetMenu(int pageNumber = 1, int dishesPerPage = 12, string? chefId = null, string? typeId = null)
        {
            try
            {
                WebClient<Menu> client = new WebClient<Menu>();
                client.Scheme = "http";
                client.Port = 5125;
                client.Host = "localhost";
                client.Path = "api/Guest/GetMenu";
                if (!(pageNumber == 1 && dishesPerPage == 12 && chefId == null && typeId == null))
                {
                    client.Path = "api/Guest/GetSortedMenu";
                    client.AddParameter("pageNumber", pageNumber.ToString());
                    client.AddParameter("amountPerPage", dishesPerPage.ToString());
                    if (chefId != null)
                        client.AddParameter("chefId", chefId.ToString());
                    if (typeId != null)
                        client.AddParameter("typeId", typeId.ToString());
                }
                Menu menu = client.Get().Result;
                return View(menu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDish(string dishId)
        {
            //WebClient<Menu> client = new WebClient<Menu>
            //   ("localhost:37878", "Guest", "GetMenu");
            //Dishes dish = client.AddParameter("dishId", dishId.ToString());
            //return View(dish);
            return View();
        }

        [HttpGet]
        public IActionResult SignUpForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Customers customer)
        {
            return View();
        }
    }
}
