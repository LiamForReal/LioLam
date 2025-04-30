using LiolamResteurent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mime;
using System.Text.Json;
using System.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

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

        public Customer GetCustomerById(string id)
        {
            try
            {
                this.dBContext.Open();//add cities and streets and house number 
                Customer customer = unitOfWorkReposetory.customerRerposetoryObject.getById(id);
                customer.city.CityName = unitOfWorkReposetory.cityRerposetoryObject.getById(customer.city.Id).CityName;
                customer.street.StreetName = unitOfWorkReposetory.streetReposetoryObject.getById(customer.street.Id).StreetName;
                return customer;
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
        public WelcomeDetails GetWelcomeDetails(string id)
        {
            try
            {
                //Console.WriteLine($"the id is: {id}");
                WelcomeDetails wD = new WelcomeDetails(); 
                this.dBContext.Open();//add cities and streets and house number 
                Customer customer = unitOfWorkReposetory.customerRerposetoryObject.getById(id);
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
                return unitOfWorkReposetory.customerRerposetoryObject.login(userName, password);
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
        public CustomerLocationView ShowSignUp()
        {
            try
            {
                CustomerLocationView registerViewModel = new CustomerLocationView();
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
        public async Task<bool> UpdateExistingUser() //user details
        {
            bool flag = false;
            bool isImageChanged = Request.Form.Files.Count > 0;
            string json = Request.Form["model"];
            Customer customer = JsonSerializer.Deserialize<Customer>(json);
            customer.CustomerImage = $"{customer.Id}{Path.GetExtension(customer.CustomerImage)}";
            try
            {    
                this.dBContext.Open();
                dBContext.BeginTransaction();
                if(!customer.CustomerImage.Contains("."))
                {
                    string savedImage = unitOfWorkReposetory.customerRerposetoryObject.getById(customer.Id).CustomerImage;
                    customer.CustomerImage = $"{customer.Id}{Path.GetExtension(savedImage)}";
                }
                Console.WriteLine($"customer Image is {customer.CustomerImage}");


                if (customer.CustomerPassword != "")
                    customer.CustomerPassword = BCrypt.Net.BCrypt.HashPassword(customer.CustomerPassword); //hash the password with salt
                else customer.CustomerPassword = unitOfWorkReposetory.customerRerposetoryObject.getById(customer.Id).CustomerPassword;

                flag = unitOfWorkReposetory.customerRerposetoryObject.update(customer);

                if (isImageChanged && flag)
                {
                    IFormFile file = Request.Form.Files[0];

                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images\Customers\");

                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Customers");
                    string fileNameWithoutExt = customer.Id; // or customer.CustomerImage if it's just the name

                    string[] possibleExtensions = { ".png", ".jpg", ".jpeg", ".webp", ".jfif" };

                    foreach (var ext in possibleExtensions)
                    {
                        string fullPath = Path.Combine(basePath, fileNameWithoutExt + ext);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    string filePath = $@"{Directory.GetCurrentDirectory()}\wwwroot\Images\Customers\{customer.CustomerImage}";
                    Console.WriteLine($"file path is: {filePath}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream); // ← Use await for proper async call
                    }
                }

                this.dBContext.Commit();
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
            Customer customer = JsonSerializer.Deserialize<Customer>(json);
            customer.CustomerPassword =  BCrypt.Net.BCrypt.HashPassword(customer.CustomerPassword); //hash the password with salt
            customer.CustomerImage=$"{customer.Id}{Path.GetExtension(customer.CustomerImage)}";
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
                        await file.CopyToAsync(fileStream);
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
            Reservation reservation = new Reservation()
            {
                ReserveDate = reserveDate,
                AmountOfPeople = amountOfPeople,
                CustomerId = CustomerId,
            };
            List<Reservation> reservations;
            try
            {
                this.dBContext.Open();
                reservations = unitOfWorkReposetory.reservationRerposetoryObject.GetByCustomer(CustomerId);
                foreach (Reservation reservationObject in reservations)
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
        public Reservation GetLastReservation(string customerId)
        {
            List<Reservation> reservations;
            DateTime dateTime = DateTime.Now;
            try
            {
                this.dBContext.Open();
                reservations = unitOfWorkReposetory.reservationRerposetoryObject.GetByCustomer(customerId);
                foreach (Reservation reservationObject in reservations)
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
        [HttpGet]
        public Order getCurrentOrderId()
        {
            try
            {
                this.dBContext.Open();
                Order order = new Order()
                {
                    Id = unitOfWorkReposetory.orderRerposetoryObject.getLastId()
                };
                return order;
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

        //[HttpPost] make it when it will be relevent
        //public bool AddDishToOrder()
        //{
            
        //    try
        //    {
        //        this.dBContext.Open();
        //        unitOfWorkReposetory.orderRerposetoryObject.
        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //        Console.WriteLine(msg);
        //        return null;
        //    }
        //    finally
        //    {
        //        this.dBContext.Close();
        //    }
        //}

        [HttpPost]
        public bool AddNewOrder() //find a way to get products 
        {
            string json = Request.Form["model"];
            Order order = JsonSerializer.Deserialize<Order>(json);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                return unitOfWorkReposetory.orderRerposetoryObject.create(order);
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
