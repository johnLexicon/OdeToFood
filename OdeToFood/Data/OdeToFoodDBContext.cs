
using Microsoft.EntityFrameworkCore;
using OdeToFood.Models;

namespace OdeToFood.Data 
{
    public class OdeToFoodDBContext : DbContext
    {

        public OdeToFoodDBContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}