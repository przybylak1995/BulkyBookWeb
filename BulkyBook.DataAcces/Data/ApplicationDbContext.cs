
using BulkyBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAcces
{
    public class ApplicationDbContext : IdentityDbContext
    {

        // Push to Database open tools en kies package manager console
        // add-migration AddCategoryToDatabase
        // Als migration oke is type volgend command
        // update-database

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Het maken van een dataset aan de hand van de model category
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<OrderHeaderModel> OrderHeaders { get; set; }
        public DbSet<OrderDetailsModel> OrderDetails { get; set; }
    }
}
