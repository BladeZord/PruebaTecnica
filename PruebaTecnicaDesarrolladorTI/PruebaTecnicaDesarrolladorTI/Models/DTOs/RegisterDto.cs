using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaDesarrolladorTI.Models.DTOs
{
    /// <summary>
    /// DTO para el registro de nuevos usuarios
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Nombre de usuario único
        /// </summary>
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 255 caracteres")]
        public string Username { get; set; } = string.Empty;



        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Confirmación de la contraseña
        /// </summary>
        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
