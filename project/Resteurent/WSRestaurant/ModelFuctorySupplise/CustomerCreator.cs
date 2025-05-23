﻿using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class CustomerCreator : IModelCreator<Customer>
    {
        /// <summary>
        /// Creates a Customer model from an IDataReader source.
        /// </summary>
        public Customer CreateModel(IDataReader src)
        {
            Customer customer = new Customer()
            {
                Id = Convert.ToString(src["CustomerId"]),
                IsOwner = Convert.ToBoolean(src["IsOwner"]), 
                CustomerUserName = Convert.ToString(src["CustomerUserName"]),
                CustomerPassword = Convert.ToString(src["CustomerPassword"]),
                CustomerImage = "http://localhost:5125/Images/Customers/" + Convert.ToString(src["CustomerImage"]),
                CustomerHouse = Convert.ToInt32(src["CustomerHouse"]),
                CustomerPhone = Convert.ToString(src["CustomerPhone"]),
                CustomerMail = Convert.ToString(src["CustomerMail"]),
                cityId = "",
                streetId = "",
                //CurrentReservation = null,
                //orders = null
            };
            return customer;
        }
    }
}
