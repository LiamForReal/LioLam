namespace LiolamResteurent
{
    public class Dishes : Models
    {
        public string DishName { get; set; }
        public int DishPrice { get; set; }
        public string DishImage { get; set; }
        public string DishDescription { get; set; }
        public List<Orders> orders { get; set; }
        public List<Chefs> chefs { get; set; }
        public List<Types> types { get; set; }

        public Dishes(string dishName, int dishPrice, string dishImage, string dishDescription)
        {
            DishName = dishName;
            DishPrice = dishPrice;
            DishImage = dishImage;
            DishDescription = dishDescription;
            orders = null;
            chefs = null;
            types = null;
        }
        public Dishes() { }
    }
}
