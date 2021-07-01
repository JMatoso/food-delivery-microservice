using Microsoft.EntityFrameworkCore;

namespace Order.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    }
}