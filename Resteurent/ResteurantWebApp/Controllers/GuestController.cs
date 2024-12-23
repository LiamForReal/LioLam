using LiolamResteurent;
using Microsoft.AspNetCore.Mvc;
using WebApiClient;

namespace ResteurantWebApp.Controllers
{
    public class GuestController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetMenu(int pageNumber = 1, int dishesPerPage = 12, string? chefId = null, string? typeId = null)
        {
            //WebClient<Menu> client = new WebClient<Menu>
            //    ("localhost:37878", "Guest", "GetMenu");
            //client.AddParameter("pageNumber", pageNumber.ToString());
            //client.AddParameter("amountPerPage", dishesPerPage.ToString());
            //if(chefId != null)
            //    client.AddParameter("chefId", chefId.ToString());
            //if(typeId != null)
            //    client.AddParameter("typeId", typeId.ToString());
            //Menu menu = await client.GetAsnc();
            //return View(menu);
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
