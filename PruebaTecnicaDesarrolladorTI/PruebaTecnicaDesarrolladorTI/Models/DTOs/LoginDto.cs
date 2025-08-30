using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaDesarrolladorTI.Models.DTOs
{
    /// <summary>
    /// DTO para las credenciales de login del usuario
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Nombre de usuario o email para el login
        /// </summary>
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string Password { get; set; } = string.Empty;
    }
}
