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
        public IActionResult GetMenu(string? chefId = null, string? typeId = null, int pageNumber = 1, int dishesPerPage = 12)
        {
            try
            {
                WebClient<Menu> client = new WebClient<Menu>();
                client.Scheme = "http";
                client.Port = 5125;
                client.Host = "localhost";
                client.Path = "api/Guest/GetMenu";
                if(chefId != null || typeId != null || pageNumber != 1 || dishesPerPage != 12)
                {
                    client.Path = "api/Guest/GetSortedMenu";
                    if (pageNumber != 1)
                    {
                        client.AddParameter("pageNumber", pageNumber.ToString());
                    }
                    if (dishesPerPage != 12)
                    {
                        client.AddParameter("amountPerPage", dishesPerPage.ToString());
                    }
                    if (chefId != null)
                        client.AddParameter("chefId", chefId);
                    if (typeId != null)
                        client.AddParameter("typeId", typeId);
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
            WebClient<Dishes> client = new WebClient<Dishes>();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Guest/GetSingleDish";
            client.AddParameter("id", dishId);
            Dishes dish = client.Get().Result;
            return View(dish);
        }

        public async Task<IActionResult> GetDishList(string? chefId = null, string? typeId = null, int pageNumber = 1 , int dishPerPage = 12)
        {
            WebClient<List<Dishes>> client = new WebClient<List<Dishes>>();
            client.Scheme = "http";
            client.Port = 5125;
            client.Host = "localhost";
            client.Path = "api/Guest/GetDishList";
            if (pageNumber != 1)
            {
                client.AddParameter("pageNumber", pageNumber.ToString());
            }
            if (dishPerPage != 12)
            {
                client.AddParameter("dishPerPage", dishPerPage.ToString());
            }
            if (chefId != null)
                client.AddParameter("chefId", chefId);
            if (typeId != null)
                client.AddParameter("typeId", typeId);
            List<Dishes> dishes = await client.Get();
            Console.WriteLine("got " + dishes.Count().ToString() + " items.");
            return PartialView(dishes);
        }
    }
}
