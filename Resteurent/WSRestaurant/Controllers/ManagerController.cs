using LiolamResteurent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace WSRestaurant.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        DBContext dBContext;
        UnitOfWorkReposetory unitOfWorkReposetory;

        public ManagerController()
        {
            this.dBContext = DBContext.GetInstance();
            this.unitOfWorkReposetory = new UnitOfWorkReposetory(this.dBContext);
        }

        [HttpPost]
        public async Task<bool> IsAdmin()
        {
            string json = Request.Form["model"];
            Customer customer = JsonSerializer.Deserialize<Customer>(json);
            Console.WriteLine(customer.CustomerUserName + " " + customer.CustomerPassword);
            try
            {
                this.dBContext.Open();//add cities and streets and house number 
                string customerId = unitOfWorkReposetory.customerRerposetoryObject.
                    CheckIfAdmin(customer.CustomerUserName, customer.CustomerPassword);
                return customerId != "";
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
        public List<Dish> GetDishes()
        {
            List<Dish> dishes;
            try
            {
                this.dBContext.Open();
                dishes = unitOfWorkReposetory.dishRerposetoryObject.getAll();
                this.dBContext.Close();
                return dishes;
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
        public bool AddNewDish()
        {
            string json = Request.Form["model"];
            Dish dish = JsonSerializer.Deserialize<Dish>(json);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.dishRerposetoryObject.create(dish);
                //dish.types = unitOfWorkReposetory.typeReposetoryObject.getByDish();
                //connection with types chefs and orders
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
        public async Task<bool> UpdateDish()
        {
            string json = Request.Form["model"];
            bool isImageChanged = Request.Form.Files.Count > 0;
            Dish dish = JsonSerializer.Deserialize<Dish>(json);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                this.dBContext.BeginTransaction();
                flag = unitOfWorkReposetory.dishRerposetoryObject.update(dish);
                if (isImageChanged)
                {
                    IFormFile file = Request.Form.Files[0];

                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images\Dishes\");

                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Customers");
                    string fileNameWithoutExt = dish.Id; // or customer.CustomerImage if it's just the name

                    string[] possibleExtensions = { ".png", ".jpg", ".jpeg", ".webp", ".jfif" };

                    foreach (var ext in possibleExtensions)
                    {
                        string fullPath = Path.Combine(basePath, fileNameWithoutExt + ext);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    string filePath = $@"{Directory.GetCurrentDirectory()}\wwwroot\Images\Customers\{dish.DishImage}";
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
        public bool DeleteDish()
        {
            string json = Request.Form["model"];
            string dishId = JsonSerializer.Deserialize<string>(json);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.dishRerposetoryObject.delete(dishId);
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
        public List<Customer> GetCustomers()
        {
            List<Customer> customers;
            try
            {
                this.dBContext.Open();
                customers = unitOfWorkReposetory.customerRerposetoryObject.getAll();
                this.dBContext.Close();
                return customers;
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

        //add city function 
        //in every add chek the parameters
        [HttpPost]
        public bool AddNewCustomer()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                Customer customer = JsonSerializer.Deserialize<Customer>(json);
                this.dBContext.Open();
                List<City> cities = unitOfWorkReposetory.cityRerposetoryObject.getAll();
                flag = unitOfWorkReposetory.customerRerposetoryObject.create(customer);
                //connection with city 
                //connection with street
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
        public bool UpdateCustomer()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                Customer customer = JsonSerializer.Deserialize<Customer>(json);
                this.dBContext.Open();
                List<City> cities = unitOfWorkReposetory.cityRerposetoryObject.getAll();
                flag = unitOfWorkReposetory.customerRerposetoryObject.update(customer);
                //connection with city 
                //connection with street
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
        public bool DeleteCustomer()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                string customerId = JsonSerializer.Deserialize<string>(json);
                this.dBContext.Open();
                if (unitOfWorkReposetory.orderRerposetoryObject.deleteByCustomer(customerId) &&
                    unitOfWorkReposetory.reservationRerposetoryObject.deleteByCustomer(customerId))
                {
                    flag = unitOfWorkReposetory.customerRerposetoryObject.delete(customerId);
                }
                else throw new Exception("return false");
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
        public List<Chef> GetChefs()
        {
            List<Chef> chefs;
            try
            {
                this.dBContext.Open();
                chefs = unitOfWorkReposetory.chefRepositoryObject.getAll();
                this.dBContext.Close();
                return chefs;
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
        public bool AddNewChef()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                Chef chef = JsonSerializer.Deserialize<Chef>(json);
                this.dBContext.Open();
                flag = unitOfWorkReposetory.chefRepositoryObject.create(chef);
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
        public bool UpdateChef()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                Chef chef = JsonSerializer.Deserialize<Chef>(json);
                this.dBContext.Open();
                flag = unitOfWorkReposetory.chefRepositoryObject.update(chef);
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
        public bool DeleteChef()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                string chefId = JsonSerializer.Deserialize<string>(json);
                this.dBContext.Open();
                flag = unitOfWorkReposetory.chefRepositoryObject.delete(chefId);
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
        public List<Order> GetOrders()
        {
            List<Order> Orders;
            try
            {
                this.dBContext.Open();
                Orders = unitOfWorkReposetory.orderRerposetoryObject.getAll();
                this.dBContext.Close();
                return Orders;
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
        public bool DeleteOrder()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                string orderId = JsonSerializer.Deserialize<string>(json);
                this.dBContext.Open();
                flag = unitOfWorkReposetory.orderRerposetoryObject.delete(orderId);
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
        public List<Reservation> GetReservations()
        {
            List<Reservation> Reservations;
            try
            {
                this.dBContext.Open();
                Reservations = unitOfWorkReposetory.reservationRerposetoryObject.getAll();
                this.dBContext.Close();
                return Reservations;
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
        public bool AddNewReservation()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                Reservation reservation = JsonSerializer.Deserialize<Reservation>(json);
                this.dBContext.Open();
                flag = unitOfWorkReposetory.reservationRerposetoryObject.create(reservation);
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
        public bool UpdateReservation()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                Reservation reservation = JsonSerializer.Deserialize<Reservation>(json);
                this.dBContext.Open();
                flag = unitOfWorkReposetory.reservationRerposetoryObject.update(reservation);
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
        public bool DeleteReservation()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                string reservationId = JsonSerializer.Deserialize<string>(json);
                this.dBContext.Open();
                flag = unitOfWorkReposetory.reservationRerposetoryObject.delete(reservationId);
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
        public List<Category> GetTypes()
        {
            List<Category> Types;
            try
            {
                this.dBContext.Open();
                Types = unitOfWorkReposetory.typeReposetoryObject.getAll();
                this.dBContext.Close();
                return Types;
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
        public bool AddNewType()
        {
            
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                Category type = JsonSerializer.Deserialize<Category>(json);
                this.dBContext.Open();
                flag = unitOfWorkReposetory.typeReposetoryObject.create(type);
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
        public bool UpdateType()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                Category type = JsonSerializer.Deserialize<Category>(json);
                this.dBContext.Open();
                flag = unitOfWorkReposetory.typeReposetoryObject.update(type);
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
        public bool Deletetype()
        {
            bool flag = false;
            try
            {
                string json = Request.Form["model"];
                string typeId = JsonSerializer.Deserialize<string>(json);
                this.dBContext.Open();
                Console.WriteLine($"{typeId} deleted type");
                flag = unitOfWorkReposetory.typeReposetoryObject.delete(typeId);
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
        public List<City> GetCities()
        {
            List<City> cities;
            try
            {
                this.dBContext.Open();
                cities = unitOfWorkReposetory.cityRerposetoryObject.getAll();
                this.dBContext.Close();
                return cities;
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
        public List<Street> GetStreets()
        {
            List<Street> streets;
            try
            {
                this.dBContext.Open();
                streets = unitOfWorkReposetory.streetReposetoryObject.getAll();
                this.dBContext.Close();
                return streets;
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


    }
}
