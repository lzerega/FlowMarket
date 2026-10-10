using Orders.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Orders.Api.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) :
             base(options)
        { }

        public DbSet<Order> OrdersDb => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                // Nombre personalizado para la tabla
                entity.ToTable("tb_order");

                // Configuración de columnas con prefijo pr_ y nombres en minúscula
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                      .HasColumnName("or_id");
                
                entity.Property(p => p.ProductId)
                      .HasColumnName("or_productId"); 

                entity.Property(p => p.Quantity)
                      .HasColumnName("or_quantity")
                      .HasPrecision(18, 2); // Buena práctica para decimales

                entity.Property(p => p.Total)
                      .HasColumnName("or_total")
                      .HasPrecision(18, 2);
            });
        }
    }
}
