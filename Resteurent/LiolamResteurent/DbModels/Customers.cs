using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Customers : Models
    {
        public string CustomerUserName { get; set; }
        public int CustomerHouse { get; set; }
        public Cities city { get; set; }
        public Streets street { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerMail { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerImage { get; set; }
        //public Reservations CurrentReservation { get; set; }
      //  public List<Orders> orders { get; set; }

        public Customers(string customerUserName, int customerHouse, string cityName, string streetName, string customerPhone, string customerMail, string customerPassword, string customerImage)
        {
            CustomerUserName = customerUserName;
            CustomerHouse = customerHouse;
            
            this.city = new Cities(cityName);
            this.street = new Streets(streetName);
            CustomerPhone = customerPhone;
            CustomerMail = customerMail;
            CustomerPassword = customerPassword;
            CustomerImage = customerImage;
            //CurrentReservation = null;
            //this.orders = null;
        }
        public Customers()
        {

        }
    }
}
