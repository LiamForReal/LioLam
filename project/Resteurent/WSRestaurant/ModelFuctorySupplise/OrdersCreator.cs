﻿using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class OrdersCreator : IModelCreator<Order>
    {
        /// <summary>
        /// Creates a Order model from an IDataReader source.
        /// </summary>
        public Order CreateModel(IDataReader src)
        {
            Order order = new Order()
            {
                Id = Convert.ToString(src["OrderId"]),
                OrderDate = Convert.ToDateTime(src["OrderDate"]), 
                CustomerId = "",
                dishes = null
            };
            return order;
        }
    }
}
