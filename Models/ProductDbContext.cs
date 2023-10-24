using Microsoft.EntityFrameworkCore;

namespace RefactorThis.Models
{
    public class ProductDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<Login> Login { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>()
            .HasKey(e => e.Name);
        }
    }
}
