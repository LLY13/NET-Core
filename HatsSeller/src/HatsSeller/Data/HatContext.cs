using HatsSeller.Models;
using Microsoft.EntityFrameworkCore;
namespace HatsSeller.Data
{
    public class HatContext : DbContext
    {
        public HatContext(DbContextOptions<HatContext> options) : base(options)
        {
        }
        public DbSet<Hat> Hats { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hat>().ToTable("Hat");
            modelBuilder.Entity<Supplier>().ToTable("Supplier");
            modelBuilder.Entity<Category>().ToTable("Category");
        }
    }
}