using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnicaDesarrolladorTI.Models
{
    /// <summary>
    /// Entidad que representa un producto en el sistema
    /// Mapea exactamente a la tabla 'producto' de MySQL
    /// </summary>
    [Table("producto")]
    public class Product
    {
        /// <summary>
        /// Identificador único del producto - mapea a id_producto
        /// </summary>
        [Key]
        [Column("id_producto")]
        public int Id { get; set; }

        /// <summary>
        /// Descripción del producto - mapea a descripcion
        /// </summary>
        [Column("descripcion")]
        [StringLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Cantidad disponible en existencia - mapea a existencia
        /// </summary>
        [Column("existencia")]
        public int? Stock { get; set; }

        /// <summary>
        /// Precio del producto - mapea a precio
        /// </summary>
        [Column("precio")]
        public double? Price { get; set; }

        /// <summary>
        /// Validaciones para asegurar datos consistentes
        /// </summary>
        [NotMapped]
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Description) && 
                       Price.HasValue && Price.Value > 0 && 
                       Stock.HasValue && Stock.Value >= 0;
            }
        }
    }
}
