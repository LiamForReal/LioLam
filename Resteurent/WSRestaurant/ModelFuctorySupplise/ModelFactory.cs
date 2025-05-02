using LiolamResteurent;
using RestaurantWebSerice;

namespace WSRestaurant
{
    public class ModelFactory
    {
        // Creators for various model types
        ChefCreator chefCreator;
        DishCreator dishCreator;
        CityCreator cityCreator;
        CustomerCreator customerCreator;
        ReservationCreator reservationCreator;
        StreetCreator streetCreator;
        TypeCreator typeCreator;
        OrdersCreator ordersCreator;
        OrderProductCreator orderProductCreator;

        /// <summary>
        /// Property to create or retrieve a ChefCreator instance.
        /// </summary>
        public ChefCreator CreateChefObject
        {
            get
            {
                if (this.chefCreator == null)
                    this.chefCreator = new ChefCreator();
                return this.chefCreator;
            }
        }

        /// <summary>
        /// Property to create or retrieve a order product Creator instance.
        /// </summary>
        public OrderProductCreator CreateOrderProductObject
        {
            get
            {
                if (this.orderProductCreator == null)
                    this.orderProductCreator = new OrderProductCreator();
                return this.orderProductCreator;
            }
        }

        /// <summary>
        /// Property to create or retrieve a DishCreator instance.
        /// </summary>
        public DishCreator createDishObject
        {
            get
            {
                if (this.dishCreator == null)
                    this.dishCreator = new DishCreator();
                return this.dishCreator;
            }
        }

        /// <summary>
        /// Property to create or retrieve a CityCreator instance.
        /// </summary>
        public CityCreator createCityObject
        {
            get
            {
                if (this.cityCreator == null)
                    this.cityCreator = new CityCreator();
                return this.cityCreator;
            }
        }

        /// <summary>
        /// Property to create or retrieve a CustomerCreator instance.
        /// </summary>
        public CustomerCreator createCustomerObject
        {
            get
            {
                if (this.customerCreator == null)
                    this.customerCreator = new CustomerCreator();
                return this.customerCreator;
            }
        }

        /// <summary>
        /// Property to create or retrieve a ReservationCreator instance.
        /// </summary>
        public ReservationCreator createReservationObject
        {
            get
            {
                if (this.reservationCreator == null)
                    this.reservationCreator = new ReservationCreator();
                return this.reservationCreator;
            }
        }

        /// <summary>
        /// Property to create or retrieve a StreetCreator instance.
        /// </summary>
        public StreetCreator createStreetObject
        {
            get
            {
                if (this.streetCreator == null)
                    this.streetCreator = new StreetCreator();
                return this.streetCreator;
            }
        }

        /// <summary>
        /// Property to create or retrieve a TypeCreator instance.
        /// </summary>
        public TypeCreator createTypeObject
        {
            get
            {
                if (this.typeCreator == null)
                    this.typeCreator = new TypeCreator();
                return this.typeCreator;
            }
        }

        /// <summary>
        /// Property to create or retrieve an OrdersCreator instance.
        /// </summary>
        public OrdersCreator createOrderObject
        {
            get
            {
                if (this.ordersCreator == null)
                    this.ordersCreator = new OrdersCreator();
                return this.ordersCreator;
            }
        }
    }
}
