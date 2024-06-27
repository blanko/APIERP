using Microsoft.EntityFrameworkCore;
using APIERP.Entidades;

namespace APIERP
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().Property(p => p.Name).HasMaxLength(60);
            modelBuilder.Entity<Category>().Property(p => p.ImageUrl).IsUnicode();

            // Mas en el futuro
        }


        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Raincheck> Rainchecks { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Error> Errores { get; set; }
    }
}
