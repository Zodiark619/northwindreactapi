using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using northwindreactapi.Models;

namespace northwindreactapi.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }
        public DbSet<Item> Items { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Item>().HasData(
                new Item
                {
                    Id=1,
                    Name="Chocolate A",
                    Price=32.43m
                },
                new Item
                {
                    Id=2,
                    Name="Rice B",
                    Price=122.3m
                },
                new Item
                {
                    Id=3,
                    Name="Soda C",
                    Price=7.99m
                }
                );
        }
    }
}
