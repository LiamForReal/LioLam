using System.Diagnostics.Metrics;

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

        public override bool Equals(object obj)
        {
            if(obj is Dish dish)
                return this.Id == dish.Id && this.DishName == dish.DishName && this.DishPrice == dish.DishPrice &&
                this.chefs.Equals(dish.chefs) && this.types.Equals(dish.types);
            return false;
        }
    }
}
