using SmitairDOTNET.Models;
using Microsoft.EntityFrameworkCore;


namespace SmitairDOTNET.DAL
{
    public class SmitairDbContext : DbContext
    {
        public SmitairDbContext(DbContextOptions<SmitairDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Effect> Effects { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
    }
}
