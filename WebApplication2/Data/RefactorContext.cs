using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class RefactorContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Laptop>().HasKey(l => l.Number);
            modelBuilder.Entity<Laptop>().HasOne(l => l.Brand).WithMany(b => b.Laptops).HasForeignKey(l => l.BrandId);
            modelBuilder.Entity<StoreLocation>().HasKey(s => s.Number);
            modelBuilder.Entity<StoreLocation>().HasMany(sl => sl.LaptopQuantities).WithOne(lq => lq.StoreLocation);
            modelBuilder.Entity<LaptopQuantity>().HasKey(lq => new { lq.LaptopNumber, lq.StoreLocationNumber });
            modelBuilder.Entity<Laptop>().Property(l => l.Price).HasColumnType("decimal(18,2)");
        }
        public RefactorContext(DbContextOptions options) : base(options) { }
        public DbSet<Laptop> Laptops { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<StoreLocation> StoreLocations { get; set; } = null!;
        public DbSet<LaptopQuantity> LaptopQuantities { get; set; } = null!;
    }
}
