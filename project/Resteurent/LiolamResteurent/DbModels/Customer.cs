using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Customer : Model
    {
        public string CustomerUserName { get; set; }
        public int CustomerHouse { get; set; }
        public string cityId { get; set; }
        public string streetId { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerMail { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerImage { get; set; }

        public bool IsOwner { get; set;}
        //public Reservations CurrentReservation { get; set; }
        //public List<Orders> orders { get; set; }

        public Customer(string customerId, bool isOwner, string customerUserName, int customerHouse, string cityId, string streetId, string customerPhone, string customerMail, string customerPassword, string customerImage)
        {
            Id = customerId;
            IsOwner = isOwner;
            CustomerUserName = customerUserName;
            CustomerHouse = customerHouse;
    
            this.cityId = cityId;
            this.streetId = streetId;
            CustomerPhone = customerPhone;
            CustomerMail = customerMail;
            CustomerPassword = customerPassword;
            CustomerImage = customerImage;
            //CurrentReservation = null;
            //this.orders = null;
        }
        public Customer(string customerUserName, bool isOwner, int customerHouse, string cityId, string streetId, string customerPhone, string customerMail, string customerPassword, string customerImage)
        {
            IsOwner = isOwner;
            CustomerUserName = customerUserName;
            CustomerHouse = customerHouse;

            this.cityId = cityId;
            this.streetId = streetId;
            CustomerPhone = customerPhone;
            CustomerMail = customerMail;
            CustomerPassword = customerPassword;
            CustomerImage = customerImage;
        }

        public Customer()
        {
        }
    }
}
