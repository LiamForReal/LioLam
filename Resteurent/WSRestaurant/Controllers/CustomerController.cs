using LiolamResteurent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mime;
using System.Text.Json;
using System.Web;

namespace WSRestaurant.Controllers
{
    [Route("api/[controller]/[action]")]
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
        public string GetLogIn(string userName, string password)
        {
            List<Customers> customers;
            try
            {
                this.dBContext.Open();//add cities and streets and house number 
                customers = unitOfWorkReposetory.customerRerposetoryObject.getAll();
                
                foreach (Customers customer in customers)
                {
                    if (customer.CustomerUserName == userName && customer.CustomerPassword == password)
                    {
                        return customer.Id;
                    }
                       
                } // get street and city names from ids!
                return "";
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
                this.dBContext.Close();
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
                Customers customer = new Customers(Id, CustomerUserName, CustomerHouse, CityId, streetId, CustomerPhone, CustomerMail, CustomerPassword, CustomerImage);
                flag = unitOfWorkReposetory.customerRerposetoryObject.update(customer);
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

        [HttpPost]
        public async Task<bool> SignUp()
        {
            try
            { //216849635
                string json = Request.Form["model"];
                IFormFile file = Request.Form.Files[0];
                Customers customer = JsonSerializer.Deserialize<Customers>(json);
                dBContext.Open();
                var customers = unitOfWorkReposetory.customerRerposetoryObject.getAll();
                foreach (var Icustomer in customers)
                {
                    if (Icustomer.Id == customer.Id) return false;
                }
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "../wwwroot/Images/Customers/");
                string filePath = uploadFolder + customer.Id + "." + Path.GetFileName(file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                };
                // Save customer to database
                bool flag = unitOfWorkReposetory.customerRerposetoryObject.create(customer);
                dBContext.Close();
                return flag;
            }
            catch (Exception ex)
            {
                return false;
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
