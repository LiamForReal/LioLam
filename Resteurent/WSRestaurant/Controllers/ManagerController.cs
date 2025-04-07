using LiolamResteurent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        [HttpGet]
        public async Task<string> IsAdmin(string userName, string password)
        {
            try
            {
                this.dBContext.Open();//add cities and streets and house number 
                string customerId = unitOfWorkReposetory.customerRerposetoryObject.CheckIfAdmin(userName, password);
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
        public List<Dishes> GetDishes()
        {
            List<Dishes> dishes;
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

        public bool AddNewDish(string dishName, string dishDescription, string DishImage, int dishPrice)
        {
            Dishes dish = new Dishes(dishName, dishPrice, DishImage, dishDescription);
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
        public bool UpdateDish(string dishId, string dishName, string dishDescription, string DishImage, int dishPrice)
        {
            Dishes dish = new Dishes(dishName, dishPrice, DishImage, dishDescription);
            dish.Id = dishId;
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.dishRerposetoryObject.update(dish);
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
        public bool DeleteDish(string dishId)
        {
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
        public List<Customers> GetCustomers()
        {
            List<Customers> customers;
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
        public bool AddNewCustomer(string CustomerId, string CustomerUserName, int CustomerHouse, string CityId, string StreetId, string CustomerPhone, string CustomerMail, string CustomerPassword, string CustomerImage)
        {
            bool flag = false;
            try
            {
                Customers customer = new Customers(CustomerId, false, CustomerUserName, CustomerHouse, CityId, StreetId, CustomerPhone, CustomerMail, CustomerPassword, CustomerImage);
                this.dBContext.Open();
                List<Cities> cities = unitOfWorkReposetory.cityRerposetoryObject.getAll();
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
        public bool UpdateCustomer(string CustomerId, string CustomerUserName, int CustomerHouse, string CityId, string streetId, string CustomerPhone, string CustomerMail, string CustomerPassword, string CustomerImage)
        {
            bool flag = false;
            try
            {
                Customers customer = new Customers(CustomerId, false, CustomerUserName, CustomerHouse, CityId, streetId, CustomerPhone, CustomerMail, CustomerPassword, CustomerImage);
                this.dBContext.Open();
                List<Cities> cities = unitOfWorkReposetory.cityRerposetoryObject.getAll();
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
        public bool DeleteCustomer(string customerId)
        {
            bool flag = false;
            try
            {
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
        public List<Chefs> GetChefs()
        {
            List<Chefs> chefs;
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
        public bool AddNewChef(string chefFirstName, string chefLastName, string chefImage)
        {
            Chefs chef = new Chefs(chefFirstName, chefLastName, chefImage);
            bool flag = false;
            try
            {
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
        public bool UpdateChef(string chefId, string chefFirstName, string chefLastName, string chefImage)
        {
            Chefs chef = new Chefs(chefFirstName, chefLastName, chefImage);
            chef.Id = chefId;
            bool flag = false;
            try
            {
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
        public bool DeleteChef(string chefId)
        {
            bool flag = false;
            try
            {
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
        public List<Orders> GetOrders()
        {
            List<Orders> Orders;
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
        public bool DeleteOrder(string orderId)
        {
            bool flag = false;
            try
            {
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
        public List<Reservations> GetReservations()
        {
            List<Reservations> Reservations;
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
        public bool AddNewReservation(string CustomerId, DateTime reserveDate, int amountOfPeople)
        {
            Reservations reservation = new Reservations(reserveDate, amountOfPeople);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                reservation.Customer = unitOfWorkReposetory.customerRerposetoryObject.getById(CustomerId);
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
        public bool UpdateReservation(string CustomerId, string reservationId, DateTime reserveDate, int amountOfPeople)
        {
            Reservations reservation = new Reservations(reserveDate, amountOfPeople);
            reservation.Id = reservationId;
            bool flag = false;
            try
            {
                this.dBContext.Open();
                reservation.Customer = unitOfWorkReposetory.customerRerposetoryObject.getById(CustomerId);
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
        public bool DeleteReservation(string reservationId)
        {
            bool flag = false;
            try
            {
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
        public List<Types> GetTypes()
        {
            List<Types> Types;
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
        public bool AddNewType(string typeName)
        {
            Types type = new Types(typeName);
            bool flag = false;
            try
            {
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
        public bool UpdateType(string typeId, string typeName)
        {
            Types type = new Types(typeName);
            type.Id = typeId;
            bool flag = false;
            try
            {
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
        public bool Deletetype(string typeId)
        {
            bool flag = false;
            try
            {
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
        public List<Cities> GetCities()
        {
            List<Cities> cities;
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
        public List<Streets> GetStreets()
        {
            List<Streets> streets;
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
