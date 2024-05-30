using Microsoft.EntityFrameworkCore;

namespace PizzaStoreNew.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    class PizzaDb : DbContext
    {
        public PizzaDb(DbContextOptions<PizzaDb> options) : base(options){ }
        public DbSet<Pizza> Pizzas { get; set; } = null!;

    }


}