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
            try
            {
                this.dBContext.Open();
                menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.getAll();
                menu.Chefs = unitOfWorkReposetory.chefRepositoryObject.getAll();
                menu.Types = unitOfWorkReposetory.typeReposetoryObject.getAll();
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
        public Menu GetSortedMenu(int pageNumber = 1,int chefId = -1, int typeId = -1, int amountPerPage = 12)
        {
            Menu menu = new Menu();
            try
            {
                
                this.dBContext.Open();
                if (chefId != -1 && typeId != -1)
                {
                    //add bought sorts
                    menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.GetByChef(chefId.ToString());
                    List<Dishes> dishesByType = unitOfWorkReposetory.dishRerposetoryObject.GetByType(typeId.ToString());
                    menu.Dishes = menu.Dishes.Except(dishesByType).ToList();
                }
                else if(chefId != -1 )
                {
                    menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.GetByChef(chefId.ToString());
                }
                else if (typeId != -1)
                {
                    menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.GetByType(typeId.ToString());
                }
                else
                {
                    menu.Dishes = unitOfWorkReposetory.dishRerposetoryObject.getAll();
                }

                int PageAmount = menu.Dishes.Count() / amountPerPage;
                if(PageAmount < pageNumber)
                {
                    throw new Exception("you dont have anough dishes to fill this page");
                }
                if (menu.Dishes.Count() <= (pageNumber - 1) * amountPerPage)
                    return null;
                menu.Dishes = menu.Dishes.GetRange((pageNumber - 1) * amountPerPage, amountPerPage);
                menu.Chefs = unitOfWorkReposetory.chefRepositoryObject.getAll();
                menu.Types = unitOfWorkReposetory.typeReposetoryObject.getAll();
                menu.PageNumber = pageNumber;
                menu.ChefId = chefId;
                menu.TypeId = typeId;
                this.dBContext.Close();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
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
