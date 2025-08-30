using Microsoft.EntityFrameworkCore;
using PruebaTecnicaDesarrolladorTI.Models;

namespace PruebaTecnicaDesarrolladorTI.Repositories.context
{
    /// <summary>
    /// Contexto de la base de datos MySQL para la aplicación
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor del contexto de base de datos
        /// </summary>
        /// <param name="options">Opciones de configuración del contexto</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet de usuarios (mapea a tabla 'usuario')
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// DbSet de productos (mapea a tabla 'producto')
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Configuración de las entidades para mapear exactamente a las tablas MySQL
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración exacta de la entidad User (tabla 'usuario')
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("id_usuario")
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                
                entity.Property(e => e.Username)
                    .HasColumnName("usuario")
                    .HasMaxLength(255)
                    .IsRequired(false);
                
                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasMaxLength(1)
                    .IsRequired(false);
                
                entity.Property(e => e.PasswordHash)
                    .HasColumnName("contrasenia")
                    .HasMaxLength(255)
                    .IsRequired(false);
            });

            // Configuración exacta de la entidad Product (tabla 'producto')
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("producto");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("id_producto")
                    .ValueGeneratedOnAdd()
                    .IsRequired();
                
                entity.Property(e => e.Description)
                    .HasColumnName("descripcion")
                    .HasMaxLength(255)
                    .IsRequired(false);
                
                entity.Property(e => e.Stock)
                    .HasColumnName("existencia")
                    .IsRequired(false);
                
                entity.Property(e => e.Price)
                    .HasColumnName("precio")
                    .IsRequired(false);
            });
        }

        /// <summary>
        /// Sobrescribe SaveChanges para validaciones adicionales
        /// </summary>
        /// <returns>Número de entidades actualizadas</returns>
        public override int SaveChanges()
        {
            ValidateEntities();
            return base.SaveChanges();
        }

        /// <summary>
        /// Sobrescribe SaveChangesAsync para validaciones adicionales
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Número de entidades actualizadas</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ValidateEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Validaciones personalizadas antes de guardar
        /// </summary>
        private void ValidateEntities()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is User user)
                {
                    // Asegurar que el estado sea válido
                    if (string.IsNullOrEmpty(user.Estado))
                        user.Estado = "A";
                }
                else if (entry.Entity is Product product)
                {
                    // Validar datos básicos del producto
                    if (entry.State == EntityState.Added && string.IsNullOrWhiteSpace(product.Description))
                        throw new InvalidOperationException("La descripción del producto es requerida");
                }
            }
        }
    }
}
