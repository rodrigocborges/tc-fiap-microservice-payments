using FIAPCloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace FIAPCloudGames.Infrastructure.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
    }
}
