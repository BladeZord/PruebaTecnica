using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaDesarrolladorTI.Models.DTOs
{
    /// <summary>
    /// DTO para la creación y actualización de productos
    /// </summary>
    public class ProductDto
    {

        /// <summary>
        /// Descripción del producto
        /// </summary>
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Precio del producto
        /// </summary>
        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        /// <summary>
        /// Cantidad en stock
        /// </summary>
        [Required(ErrorMessage = "El stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }
    }

    /// <summary>
    /// DTO para la respuesta de productos con información adicional
    /// </summary>
    public class ProductResponseDto
    {
        /// <summary>
        /// Identificador del producto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descripción del producto
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Precio del producto
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Cantidad en stock
        /// </summary>
        public int Stock { get; set; }
    }
}
