using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using LiolamResteurent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WSRestaurant
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        DBContext dBContext;
        UnitOfWorkReposetory unitOfWorkReposetory;
        
        public GuestController()
        {
            this.dBContext = DBContext.GetInstance();
            this.unitOfWorkReposetory = new UnitOfWorkReposetory(dBContext);
        }

        [HttpGet]

        //Menu is a view model of the oppening screan
        public Menu GetMenu()
        {
            Menu menu = new Menu();
            int totalPages = 0;
            try
            {
                this.dBContext.Open();
                menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.getAll();
                menu.Chefs = unitOfWorkReposetory.chefRepositoryObject.getAll();
                menu.Types = unitOfWorkReposetory.typeReposetoryObject.getAll();

                totalPages = menu.Dishes.Count() / 12; 
                if (menu.Dishes.Count() % 12 != 0)
                    totalPages++;
                
                menu.totalPages = totalPages;
                menu.Dishes = menu.Dishes.Take(12).ToList();
                menu.PageNumber = 1;
                menu.ChefId = -1;
                menu.TypeId = -1;
                this.dBContext.Close();
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
            return menu;
        }

        [HttpGet]
        public Menu GetSortedMenu(string? chefId = null, string? typeId = null, int pageNumber = 1, int amountPerPage = 12)
        {
            Menu menu = new Menu();
            int newAmount;
            try
            {
                menu.ChefId = -1;
                menu.TypeId = -1;
                this.dBContext.Open();
                if (chefId != null && typeId != null)
                {
                    //add bought sorts
                    menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.GetByChef(chefId);
                    List<Dishes> dishesByType = unitOfWorkReposetory.dishRerposetoryObject.GetByType(typeId);
                    menu.Dishes = menu.Dishes.Except(dishesByType).ToList();
                }
                else if(chefId != null)
                {
                    menu.ChefId = int.Parse(chefId);
                    menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.GetByChef(chefId);
                }
                else if (typeId != null)
                {
                    menu.TypeId = int.Parse(typeId);
                    menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.GetByType(typeId);
                }
                else
                {
                    menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.getAll();
                }
                int PageAmount = menu.Dishes.Count() / amountPerPage;
                if (menu.Dishes.Count % 12 != 0)
                {
                    PageAmount++;
                }

                if (PageAmount < (pageNumber - 1))
                {
                    throw new Exception("you dont have anough dishes to fill this page");
                }
                if(menu.Dishes.Count() < amountPerPage * pageNumber)
                {
                    newAmount = menu.Dishes.Count() % amountPerPage;
                    menu.Dishes = menu.Dishes.GetRange((pageNumber - 1) * amountPerPage, newAmount);
                }
                else menu.Dishes = menu.Dishes.GetRange((pageNumber - 1) * amountPerPage, amountPerPage);

                menu.totalPages = PageAmount;
                menu.Chefs = unitOfWorkReposetory.chefRepositoryObject.getAll();
                menu.Types = unitOfWorkReposetory.typeReposetoryObject.getAll();
                menu.PageNumber = pageNumber;
                this.dBContext.Close();
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
            return menu;
        }

        [HttpGet]
        public Dishes GetSingleDish(string id)
        {
            Dishes dish;
            try
            {
                this.dBContext.Open();
                dish = unitOfWorkReposetory.dishRerposetoryObject.getById(id);
                dish.types = unitOfWorkReposetory.typeReposetoryObject.getByDish(id);
                dish.chefs = unitOfWorkReposetory.chefRepositoryObject.GetByDish(id);
                this.dBContext.Close();
                return dish;
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
