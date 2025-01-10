using Microsoft.AspNetCore.Mvc;
using LiolamResteurent;
using WebApiClient;

namespace ResteurantWebApp.Controllers
{
    public class GuestController : Controller
    {
        [HttpGet]
        public IActionResult GetDifaultScreen()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetMenu(int pageNumber = 1, int dishesPerPage = 12, string? chefId = null, string? typeId = null)
        {
            WebClient<Menu> client = new WebClient<Menu>();
            client.Scheme = "http";
            client.port = 5125;
            client.host = "localhost/api/GuestController/GetMenu";
            client.path = "api/GuestController/GetMenu";
            Menu menu = client.Get().Result;
            client.AddParameter("pageNumber", pageNumber.ToString());
            client.AddParameter("amountPerPage", dishesPerPage.ToString());
            if (chefId != null)
                client.AddParameter("chefId", chefId.ToString());
            if (typeId != null)
                client.AddParameter("typeId", typeId.ToString());
            return View(menu);
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
