using LiolamResteurent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

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
            try
            {
                this.dBContext.Open();//add cities and streets and house number 
                string customerId = unitOfWorkReposetory.customerRerposetoryObject.CheckIfAdmin(customer.CustomerUserName, customer.CustomerPassword);
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
        public bool IsDishExist(string dishName)
        {
            try
            {
                this.dBContext.Open();
                Console.WriteLine("here");
                Dish dish = unitOfWorkReposetory.dishRerposetoryObject.getByName(dishName);
                return dish != null;
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

        [HttpGet]
        public AddDishView GetAddDishView()
        {
            try
            {
                this.dBContext.Open();
                AddDishView addDishView = new AddDishView()
                {
                    types = this.unitOfWorkReposetory.typeReposetoryObject.getAll(),
                    chefs = this.unitOfWorkReposetory.chefRepositoryObject.getAll()
                };

                return addDishView;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                this.dBContext.Close();
            }
        }

        [HttpPost]
        public async Task<bool> AddNewDish()
        {
            string json = Request.Form["model"];
            IFormFile file = Request.Form.Files[0];
            Dish dish = JsonSerializer.Deserialize<Dish>(json);
            dish.DishImage = Path.GetExtension(dish.DishImage);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                this.dBContext.BeginTransaction();
                flag = unitOfWorkReposetory.dishRerposetoryObject.create(dish);
                if (flag)
                { 
                    string filePath = $@"{Directory.GetCurrentDirectory()}\wwwroot\Images\Dishes\{dish.DishImage}";
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    };
                }
                //dish.types = unitOfWorkReposetory.typeReposetoryObject.getByDish();
                //connection with types chefs and orders
                this.dBContext.Commit();
                return flag;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                this.dBContext.Rollback();
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
            bool isImageExist = Request.Form.Files.Count > 0;
            Dish dish = JsonSerializer.Deserialize<Dish>(json);
            dish.DishImage = $"{dish.Id}{Path.GetExtension(dish.DishImage)}";
            bool flag = false;
            try
            {
                this.dBContext.Open();
                this.dBContext.BeginTransaction();
                string savedImage = unitOfWorkReposetory.dishRerposetoryObject.getById(dish.Id).DishImage;
                if (!isImageExist)
                    dish.DishImage = $"{dish.Id}{Path.GetExtension(savedImage)}";

                flag = unitOfWorkReposetory.dishRerposetoryObject.update(dish);

                if (isImageExist && flag)
                {
                    IFormFile file = Request.Form.Files[0];

                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Dishes");
            
                    string fullPath = Path.Combine(basePath, savedImage);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string filePath = $@"{Directory.GetCurrentDirectory()}\wwwroot\Images\Dishes\{dish.DishImage}";
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
                this.dBContext.Rollback();
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
                this.dBContext.BeginTransaction();
                Dish dish = unitOfWorkReposetory.dishRerposetoryObject.getById(dishId);
                string imgName = $"{dish.Id}{Path.GetExtension(dish.DishImage)}";
                dish = null;
                flag = unitOfWorkReposetory.dishRerposetoryObject.delete(dishId);
                if(flag)
                {
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Dishes");
                    string fullPath = Path.Combine(basePath, imgName);
                    Console.WriteLine(fullPath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                
                this.dBContext.Commit(); 
                return flag;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                this.dBContext.Rollback();
                return false;
            }
            finally
            {
                this.dBContext.Close();
            }
        }

        [HttpGet]
        public bool IsCustomerExist(string userName)
        {
            try
            {
                this.dBContext.Open();
                Customer customer = unitOfWorkReposetory.customerRerposetoryObject.getByName(userName);
                return customer != null;
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
        public async Task<bool> AddNewCustomer()
        {
            string json = Request.Form["model"];
            IFormFile file = Request.Form.Files[0];
            Customer customer = JsonSerializer.Deserialize<Customer>(json);
            customer.CustomerPassword = BCrypt.Net.BCrypt.HashPassword(customer.CustomerPassword);
            customer.CustomerImage = Path.GetExtension(customer.CustomerImage);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                flag = unitOfWorkReposetory.customerRerposetoryObject.create(customer);
                if (flag)
                {
                    string filePath = $@"{Directory.GetCurrentDirectory()}\wwwroot\Images\Customers\{customer.CustomerImage}";
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    };
                }
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
        public async Task<bool> UpdateCustomer()
        {
            string json = Request.Form["model"];
            bool isImageExist = Request.Form.Files.Count > 0;
            Customer customer = JsonSerializer.Deserialize<Customer>(json);
            customer.CustomerImage = $"{customer.Id}{Path.GetExtension(customer.CustomerImage)}";
            bool flag = false;
            try
            {
                this.dBContext.Open();
                this.dBContext.BeginTransaction();
                string savedImage = unitOfWorkReposetory.customerRerposetoryObject.getById(customer.Id).CustomerImage;
                if (customer.CustomerPassword != "")
                    customer.CustomerPassword = BCrypt.Net.BCrypt.HashPassword(customer.CustomerPassword);
                if (!isImageExist)
                    customer.CustomerImage = $"{customer.Id}{Path.GetExtension(savedImage)}";

                flag = unitOfWorkReposetory.customerRerposetoryObject.update(customer);
                if (isImageExist && flag)
                {
                    IFormFile file = Request.Form.Files[0];

                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Customers");

                    string fullPath = Path.Combine(basePath, savedImage);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
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
                this.dBContext.Rollback();
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
            string json = Request.Form["model"];
            string customerId = JsonSerializer.Deserialize<string>(json);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                Customer customer = unitOfWorkReposetory.customerRerposetoryObject.getById(customerId);
                string imgName = $"{customer.Id}{Path.GetExtension(customer.CustomerImage)}";
                customer = null;
                flag = unitOfWorkReposetory.customerRerposetoryObject.delete(customerId);
                if (flag)
                {
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Customers");
                    string fullPath = Path.Combine(basePath, imgName);
                    Console.WriteLine(fullPath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
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

        [HttpGet]
        public bool IsChefExist(string firstName, string lastName)
        {
            try
            {
                this.dBContext.Open();
                Chef chef = unitOfWorkReposetory.chefRepositoryObject.getByName(firstName, lastName);
                return chef != null;
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
        public Chef GetChefById(string id)
        {
            try
            {
                this.dBContext.Open();
                return unitOfWorkReposetory.chefRepositoryObject.getById(id);
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
        public async Task<bool> AddNewChef()
        {
            string json = Request.Form["model"];
            IFormFile file = Request.Form.Files[0];
            Chef chef = JsonSerializer.Deserialize<Chef>(json);
            chef.ChefImage = Path.GetExtension(chef.ChefImage);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                this.dBContext.BeginTransaction();
                flag = unitOfWorkReposetory.chefRepositoryObject.create(chef);
                if (flag)
                {
                    string filePath = $@"{Directory.GetCurrentDirectory()}\wwwroot\Images\Chefs\{chef.ChefImage}";
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    };
                }
                this.dBContext.Commit();
                return flag;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                this.dBContext.Rollback();
                return false;
            }
            finally
            {
                this.dBContext.Close();
            }
        }


        [HttpPost]
        public async Task<bool> UpdateChef()
        {
            string json = Request.Form["model"];
            bool isImageExsist = Request.Form.Files.Count > 0;
            Chef chef = JsonSerializer.Deserialize<Chef>(json);
            chef.ChefImage = $"{chef.Id}{Path.GetExtension(chef.ChefImage)}";
            bool flag = false;
            try
            {
                this.dBContext.Open();
                string savedImage = unitOfWorkReposetory.chefRepositoryObject.getById(chef.Id).ChefImage;
                if (!isImageExsist)
                    chef.ChefImage = $"{chef.Id}{Path.GetExtension(savedImage)}";


                flag = unitOfWorkReposetory.chefRepositoryObject.update(chef);

                if (isImageExsist && flag)
                {
                    IFormFile file = Request.Form.Files[0];

                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Chefs");

                    string fullPath = Path.Combine(basePath, savedImage);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string filePath = $@"{Directory.GetCurrentDirectory()}\wwwroot\Images\Chefs\{chef.ChefImage}";
                    Console.WriteLine($"file path is: {filePath}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream); // Use await for proper async call
                    }
                }
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
            string json = Request.Form["model"];
            string chefId = JsonSerializer.Deserialize<string>(json);
            bool flag = false;
            try
            {
                this.dBContext.Open();
                this.dBContext.BeginTransaction();
                Chef chef = unitOfWorkReposetory.chefRepositoryObject.getById(chefId);
                string imgName = $"{chef.Id}{Path.GetExtension(chef.ChefImage)}";
                chef = null;
                flag = unitOfWorkReposetory.chefRepositoryObject.delete(chefId);
                if (flag)
                {
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Chefs");
                    string fullPath = Path.Combine(basePath, imgName);
                    Console.WriteLine(fullPath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                this.dBContext.Commit();
                return flag;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                this.dBContext.Rollback();
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
        public Category GetTypeById(string id)
        {
            try
            {
                this.dBContext.Open();
                return unitOfWorkReposetory.typeReposetoryObject.getById(id);
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

        [HttpGet]
        public bool IsTypeExist(string typeName)
        {
            try
            {
                this.dBContext.Open();
                Category type = unitOfWorkReposetory.typeReposetoryObject.getByName(typeName);
                return type != null;
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
            string json = Request.Form["model"];
            string typeId = JsonSerializer.Deserialize<string>(json);
            try
            {
                this.dBContext.Open();
                this.dBContext.BeginTransaction();
                flag = unitOfWorkReposetory.typeReposetoryObject.delete(typeId);
                this.dBContext.Commit();
                return flag;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
                this.dBContext.Rollback();
                return false;
            }
            finally
            {
                this.dBContext.Close();
            }
        }

        [HttpGet]
        public CustomerLocationView GetUpdateCustomerView()
        {
            CustomerLocationView updateCustomerView = new CustomerLocationView();
            try
            {
                this.dBContext.Open();
                updateCustomerView.Cities = unitOfWorkReposetory.cityRerposetoryObject.getAll();
                updateCustomerView.Streets = unitOfWorkReposetory.streetReposetoryObject.getAll();
                return updateCustomerView;
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
