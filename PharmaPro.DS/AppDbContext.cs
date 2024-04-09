using Bogus;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Domain.Categories;
using PharmaPro.Domain.Products;
using PharmaPro.Domain.Storage;
using PharmaPro.Domain.Users;
using PharmaPro.DS.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<ImageStorage> ImagesStorage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)"); 
        }


        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var categories = GenerateCategories(8);
            var products = GenerateProducts(categories, 350);

            modelBuilder.Entity<Category>().HasData(categories);
            modelBuilder.Entity<Product>().HasData(products);
        }

        private static List<Category> GenerateCategories(int count)
        {
            var categories = new List<Category>();
            var faker = new Faker();

            var pharmaceuticalCategories = new List<string>
            {
            "Pain Relief",
            "Cold & Flu",
            "Digestive Health",
            "Allergy & Sinus",
            "First Aid",
            "Skin Treatments",
            "Vitamins & Supplements",
            "Prescription Medications"
            };

            for (int i = 0; i < count; i++)
            {
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = pharmaceuticalCategories[i % pharmaceuticalCategories.Count] // Assign pharmaceutical categories in a loop
                };
                categories.Add(category);
            }

            return categories;
        }

        private static List<Product> GenerateProducts(List<Category> categories, int count)
        {
            var products = new List<Product>();
            var faker = new Faker();

            for (int i = 0; i < count; i++)
            {
                var category = categories[faker.Random.Int(0, categories.Count - 1)]; // Randomly select a category
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = faker.Commerce.ProductName(),
                    Description = faker.Commerce.ProductAdjective(),
                    Amount = faker.Random.Int(1, 100),
                    BarCode = faker.Commerce.Ean13(),
                    Active = faker.Random.Bool(),
                    SoldOut = faker.Random.Bool(),
                    Price = faker.Random.Decimal(1, 1000),
                    CategoryId = category.Id
                };
                products.Add(product);
            }

            return products;
        }
        */


    }
}
