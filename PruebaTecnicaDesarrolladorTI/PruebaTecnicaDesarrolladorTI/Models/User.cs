using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnicaDesarrolladorTI.Models
{
    /// <summary>
    /// Entidad que representa un usuario del sistema
    /// Mapea exactamente a la tabla 'usuario' de MySQL
    /// </summary>
    [Table("usuario")]
    public class User
    {
        /// <summary>
        /// Identificador único del usuario - mapea a id_usuario
        /// </summary>
        [Key]
        [Column("id_usuario")]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de usuario - mapea a usuario
        /// </summary>
        [Column("usuario")]
        [StringLength(255)]
        public string? Username { get; set; }

        /// <summary>
        /// Estado del usuario - mapea a estado
        /// </summary>
        [Column("estado")]
        [StringLength(1)]
        public string? Estado { get; set; }

        /// <summary>
        /// Hash de la contraseña - mapea a contrasenia
        /// </summary>
        [Column("contrasenia")]
        [StringLength(255)]
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Propiedad calculada para verificar si el usuario está activo
        /// </summary>
        [NotMapped]
        public bool IsActive 
        { 
            get => Estado == "A"; 
            set => Estado = value ? "A" : "I"; 
        }

        /// <summary>
        /// Email del usuario (no está en la tabla, se usa solo para validaciones)
        /// </summary>
        [NotMapped]
        public string Email { get; set; } = string.Empty;
    }
}
