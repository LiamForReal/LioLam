using LiolamResteurent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public Customers GetLogIn(string userName, string password)
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
                        customer.cityId = int.Parse(unitOfWorkReposetory.cityRerposetoryObject.getByCustomer(customer.Id).Id);
                        customer.streetId = int.Parse(unitOfWorkReposetory.streetReposetoryObject.getByCustomer(customer.Id).Id);
                        this.dBContext.Close();
                        return customer;
                    }
                       
                } // get street and city names from ids!
                return null;
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
        public bool UpdateExistingUser(string CustomerUserName, int CustomerHouse, int CityId, int streetId, string CustomerPhone, string CustomerMail, string CustomerPassword, string CustomerImage) //user details
        {
            bool flag = false;
            try
            {    
                this.dBContext.Open();
                Customers customer = new Customers(CustomerUserName, CustomerHouse, CityId, streetId, CustomerPhone, CustomerMail, CustomerPassword, CustomerImage);
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
        public string signUp(string customerId, string CustomerUserName, int CustomerHouse, int CityId, int StreetId, string CustomerPhone, string CustomerMail, string CustomerPassword, string CustomerImage) 
        {
            bool flag = false;
            try
            {
                this.dBContext.Open();
                List<Customers> customers = unitOfWorkReposetory.customerRerposetoryObject.getAll();
                foreach(Customers Icustomer in customers)
                {
                    if(Icustomer.Id == customerId || Icustomer.CustomerUserName == CustomerUserName)
                    {
                        return null;
                    }
                }
                Customers customer = new Customers(customerId, CustomerUserName, CustomerHouse, CityId, StreetId, CustomerPhone, CustomerMail, CustomerPassword, CustomerImage);
                List<Cities> cities = unitOfWorkReposetory.cityRerposetoryObject.getAll();
                flag = unitOfWorkReposetory.customerRerposetoryObject.create(customer);
                //connection with city 
                //connection with street
                this.dBContext.Close();
                if (flag == false)
                    return null;
                return customerId;
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
