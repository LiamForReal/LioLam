using LiolamResteurent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WSRestaurant.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        DBContext dBContext;
        UnitOfWorkReposetory unitOfWorkReposetory;

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

        public bool AddNewDish(Dishes dish)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.dishRerposetoryObject.create(dish);
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
        public bool UpdateDish(string dishId)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.dishRerposetoryObject.update(unitOfWorkReposetory.dishRerposetoryObject.getById(dishId));
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

        [HttpPost]
        public bool AddNewCustomer(Customers customer)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.customerRerposetoryObject.create(customer);
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
        public bool UpdateCustomer(string CustomerId)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.dishRerposetoryObject.update(unitOfWorkReposetory.dishRerposetoryObject.getById(CustomerId));
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
                flag = unitOfWorkReposetory.dishRerposetoryObject.delete(customerId);
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
        public bool AddNewChef(Chefs chef)
        {
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
        public bool UpdateChef(string chefId)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.chefRepositoryObject.update(unitOfWorkReposetory.chefRepositoryObject.getById(chefId));
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
                flag = unitOfWorkReposetory.dishRerposetoryObject.delete(chefId);
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
        public bool AddNewOrder(Orders order)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
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

        [HttpPost]
        public bool UpdateOrder(string orderId)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.orderRerposetoryObject.update(unitOfWorkReposetory.orderRerposetoryObject.getById(orderId));
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
        public bool DeleteOrder(string orderId)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.dishRerposetoryObject.delete(orderId);
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

        public bool AddNewReservation(Reservations reservation)
        {
            bool flag = false;
            try
            {
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
        public bool UpdateReservation(string reservationId)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.reservationRerposetoryObject.update(unitOfWorkReposetory.reservationRerposetoryObject.getById(reservationId));
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
                flag = unitOfWorkReposetory.dishRerposetoryObject.delete(reservationId);
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
        public bool AddNewType(Types type)
        {
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
        public bool Updatetype(string typeId)
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.typeReposetoryObject.update(unitOfWorkReposetory.typeReposetoryObject.getById(typeId));
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
                flag = unitOfWorkReposetory.dishRerposetoryObject.delete(typeId);
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
