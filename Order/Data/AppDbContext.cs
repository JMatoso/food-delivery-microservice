using Microsoft.EntityFrameworkCore;
using Order.DTO;

namespace Order.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<Cart> Cart { get; set; }
        public DbSet<DTO.Order> Orders { get; set; }
    }
}