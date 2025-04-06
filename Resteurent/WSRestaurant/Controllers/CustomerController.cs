using LiolamResteurent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mime;
using System.Text.Json;
using System.Web;

namespace WSRestaurant.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        DBContext dBContext;
        UnitOfWorkReposetory unitOfWorkReposetory;

        public CustomerController()
        {
            this.dBContext = DBContext.GetInstance();
            this.unitOfWorkReposetory = new UnitOfWorkReposetory(this.dBContext);
        }

        [HttpGet]
        public welcomeDetails GetWelcomeDetails(string id)
        {
            try
            {
                //Console.WriteLine($"the id is: {id}");
                welcomeDetails wD = new welcomeDetails(); 
                this.dBContext.Open();//add cities and streets and house number 
                Customers customer = unitOfWorkReposetory.customerRerposetoryObject.getById(id);
                wD.name = customer.CustomerUserName;
                wD.image = customer.CustomerImage;
                customer = null;
                return wD;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                return null;
            }
            finally
            {
                this.dBContext.Close();
            }
        }

        [HttpGet]
        public string LogIn(string userName, string password)
        {
           
            try
            {
                this.dBContext.Open();//add cities and streets and house number 
                string customerId = unitOfWorkReposetory.customerRerposetoryObject.GetCustomerId(userName, password);
                return customerId;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                return "";
            }
            finally
            {
                this.dBContext.Close();
            }
        }

        [HttpGet]
        public registerViewModel ShowSignUp()
        {
            try
            {
                registerViewModel registerViewModel = new registerViewModel();
                this.dBContext.Open();
                registerViewModel.Cities = unitOfWorkReposetory.cityRerposetoryObject.getAll();
                registerViewModel.Streets = unitOfWorkReposetory.streetReposetoryObject.getAll();
                return registerViewModel;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                return null;
            }
            finally
            {
                this.dBContext.Close();
            }
        }

        [HttpPost]
        public bool UpdateExistingUser(string Id, string CustomerUserName, int CustomerHouse, int CityId, int streetId, string CustomerPhone, string CustomerMail, string CustomerPassword, string CustomerImage, IFormFile pickture) //user details
        {
            bool flag = false;
            try
            {    
                this.dBContext.Open();
                Customers customer = new Customers(Id, false, CustomerUserName, CustomerHouse, CityId, streetId, CustomerPhone, CustomerMail, CustomerPassword, CustomerImage);
                flag = unitOfWorkReposetory.customerRerposetoryObject.update(customer);
                return flag;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                return false;
            }
            finally
            {
                this.dBContext.Close();
            }
        }

        [HttpPost]
        public async Task<bool> SignUp()
        {
            string json = Request.Form["model"];
            IFormFile file = Request.Form.Files[0];
            Customers customer = JsonSerializer.Deserialize<Customers>(json);
            customer.CustomerImage=$"{customer.Id}{ Path.GetExtension(customer.CustomerImage)}";
            try
            { //216849635
              
                dBContext.Open();
                dBContext.BeginTransaction();
                bool flag = unitOfWorkReposetory.customerRerposetoryObject.create(customer);
                if(flag)
                {
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), @"\wwwroot\Images\Customers\");
                    string filePath =$@"{Directory.GetCurrentDirectory()}\wwwroot\Images\Customers\{customer.CustomerImage}";
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyToAsync(fileStream);
                    };
                }

                // Save customer to database
                this.dBContext.Commit();
              
                return true;
            }
            catch (Exception ex)
            {
                this.dBContext.Rollback();
                return false;
            }
            finally
            {
                this.dBContext.Close();
            }
            return false;
        }

        [HttpPost]
        public bool ScheduleReservation(DateTime reserveDate, int amountOfPeople, string CustomerId)
        {
            bool flag = false;
            Reservations reservation = new Reservations(reserveDate, amountOfPeople);
            List<Reservations> reservations;
            try
            {
                this.dBContext.Open();
                reservation.Customer = unitOfWorkReposetory.customerRerposetoryObject.getById(CustomerId);
                reservations = unitOfWorkReposetory.reservationRerposetoryObject.GetByCustomer(CustomerId);
                foreach (Reservations reservationObject in reservations)
                {
                    if (reservationObject.ReserveDate >= DateTime.Now.Date)
                    {
                        if ((int.Parse)(reservationObject.ReserveHour) < DateTime.Now.TimeOfDay.Hours)
                        {
                            flag = unitOfWorkReposetory.reservationRerposetoryObject.create(reservation);
                        }
                    }

                }
                this.dBContext.Close();
                return flag;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                return false;
            }
            finally
            {
                this.dBContext.Close();
            }
        }

        [HttpGet]
        public Reservations GetLastReservation(string customerId)
        {
            List<Reservations> reservations;
            DateTime dateTime = DateTime.Now;
            try
            {
                this.dBContext.Open();
                reservations = unitOfWorkReposetory.reservationRerposetoryObject.GetByCustomer(customerId);
                foreach (Reservations reservationObject in reservations)
                {

                    if (reservationObject.ReserveDate > dateTime)
                    {
                        dateTime = reservationObject.ReserveDate;
                    }
                }
                return unitOfWorkReposetory.reservationRerposetoryObject.GetByDate(dateTime, reservations);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                return null;
            }
            finally
            {
                this.dBContext.Close();
            }
        }

        [HttpPost]
        public bool AddNewOrder(string CustomerId, DateTime date) //find a way to get products 
        {
            Orders order = new Orders(date);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                order.Customer = unitOfWorkReposetory.customerRerposetoryObject.getById(CustomerId);
                flag = unitOfWorkReposetory.orderRerposetoryObject.create(order);
                this.dBContext.Close();
                return flag;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                return false;
            }
            finally
            {
                this.dBContext.Close();
            }
        }
    }
}
