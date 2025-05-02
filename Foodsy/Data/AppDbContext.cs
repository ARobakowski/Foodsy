namespace Foodsy.Data
{
    using Foodsy.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
       
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); 

            
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Burger",
                    Price = 5.99m,
                    Description = "Juicy beef burger",
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "Pizza",
                    Price = 8.99m,
                    Description = "Cheese and tomato pizza",
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Pasta",
                    Price = 7.99m,
                    Description = "Spaghetti with marinara sauce",
                },
                new MenuItem
                {
                    Id = 4,
                    Name = "Pierogies",
                    Price = 21.50m,
                    Description = "Pierogies with strawberries",
                },
                new MenuItem
                {
                    Id = 5,
                    Name = "Salad",
                    Price = 4.99m,
                    Description = "Fresh garden salad",
                },
                new MenuItem
                {
                    Id = 6,
                    Name = "Soup",
                    Price = 3.99m,
                    Description = "Chicken noodle soup",
                },
                new MenuItem
                {
                    Id = 7,
                    Name = "Sandwich",
                    Price = 6.99m,
                    Description = "Turkey and cheese sandwich",
                },
                new MenuItem
                {
                    Id = 8,
                    Name = "Taco",
                    Price = 2.99m,
                    Description = "Beef taco",
                }
            );
        }
    }
}
