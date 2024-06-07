using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Domain.Categories;
using PharmaPro.Domain.Contacts;
using PharmaPro.Domain.Orders;
using PharmaPro.Domain.Products;
using PharmaPro.Domain.Storage;
using PharmaPro.Domain.UserProducts;
using PharmaPro.Domain.Users;

namespace PharmaPro.DS
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ImageStorage> ImagesStorage { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderProducts> orderProducts { get; set; }
        public DbSet<UserProduct> userProducts { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProduct>()
                .HasKey(up => new { up.UserId, up.ProductId });

            modelBuilder.Entity<OrderProducts>()
                .HasKey(up => new { up.OrderId, up.ProductId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
