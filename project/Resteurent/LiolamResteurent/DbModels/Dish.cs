namespace LiolamResteurent
{
    public class Dish : Model
    {
        public string DishName { get; set; }
        public int DishPrice { get; set; }
        public string DishImage { get; set; }
        public string DishDescription { get; set; }
       // public List<Orders> orders { get; set; }
        public List<Chef> chefs { get; set; }
        public List<Category> types { get; set; }

        public Dish(string dishName, int dishPrice, string dishImage, string dishDescription)
        {
            DishName = dishName;
            DishPrice = dishPrice;
            DishImage = dishImage;
            DishDescription = dishDescription;
            chefs = null;
            types = null;
           // orders = null;
        }

        public Dish()
        {
        }
    }
}
