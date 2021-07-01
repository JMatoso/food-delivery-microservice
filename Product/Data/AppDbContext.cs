using Microsoft.EntityFrameworkCore;
using Product.DTO;

namespace Product.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<DTO.Product> Products { get; set; }
        public DbSet<Extra> Extras { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}