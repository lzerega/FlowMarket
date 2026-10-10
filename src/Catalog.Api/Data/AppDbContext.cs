using Catalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
             base(options)
        { }

        public DbSet<Product> ProductsDb { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                // Nombre personalizado para la tabla
                entity.ToTable("tb_product");

                // Configuración de columnas con prefijo pr_ y nombres en minúscula
                entity.Property(p => p.Id)
                      .HasColumnName("pr_id");

                entity.Property(p => p.Name)
                      .HasColumnName("pr_name")
                      .HasMaxLength(150); // Opcional: especificar longitud

                entity.Property(p => p.Price)
                      .HasColumnName("pr_price")
                      .HasPrecision(18, 2); // Buena práctica para decimales

                entity.Property(p => p.Stock)
                      .HasColumnName("pr_stock");
            });
        }
    }
}
