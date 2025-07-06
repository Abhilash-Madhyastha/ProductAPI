using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); 
            modelBuilder.Entity<Products>()
                .Property(p => p.ProductId)
                .UseIdentityColumn(100000, 1); 
            modelBuilder.Entity<Products>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Products>()
                .Property(p => p.UpdatedAt)
                .IsRequired(false);

        }
    }  
}
