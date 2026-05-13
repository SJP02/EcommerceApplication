using EcommerceApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApplication.Data
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Category> Categories {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.CategoryId);
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(20);;
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Price);
                entity.HasIndex(e => e.CompanyId);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Price).HasPrecision(10, 2);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.CompanyId);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasOne(e => e.ProductCompany)
                      .WithMany(c => c.ProductList)
                      .HasForeignKey(e => e.CompanyId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.ProductCategory)
                      .WithMany(c => c.ProductList)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasIndex(e => e.CompanyName);
                entity.HasIndex(e => e.Location);
                entity.HasKey(e => e.CompanyId);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.Location).HasMaxLength(15);
                entity.Property(e => e.PhoneNumber).IsRequired();
                entity.Property(e => e.EstablishedYear).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);

            });
        }
        
    }
}
