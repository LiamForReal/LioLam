namespace LiolamResteurent
{
    public class Dish : IModel
    {
        public string DishName { get; set; }
        public int DishPrice { get; set; }
        public string DishImage { get; set; }
        public string DishDescription { get; set; }
        public List<Chef> chefs { get; set; }
        public List<Category> types { get; set; }
    }
}
