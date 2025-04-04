using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Customers : Models
    {
        public string CustomerUserName { get; set; }
        public int CustomerHouse { get; set; }
        public int cityId { get; set; }
        public int streetId { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerMail { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerImage { get; set; }

        public bool IsOwner { get; set;}
        //public Reservations CurrentReservation { get; set; }
        //public List<Orders> orders { get; set; }

        public Customers(string customerId, bool isOwner, string customerUserName, int customerHouse, int cityId, int streetId, string customerPhone, string customerMail, string customerPassword, string customerImage)
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
        public Customers(string customerUserName, bool isOwner, int customerHouse, int cityId, int streetId, string customerPhone, string customerMail, string customerPassword, string customerImage)
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

        public Customers()
        {
        }
    }
}
