namespace PruebaTecnicaDesarrolladorTI.Models.DTOs
{
    /// <summary>
    /// DTO para la respuesta de autenticación exitosa
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// Token JWT generado
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de expiración del token
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Información del usuario autenticado
        /// </summary>
        public UserInfoDto User { get; set; } = new();
    }

    /// <summary>
    /// DTO con información básica del usuario
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// ID del usuario
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public string Username { get; set; } = string.Empty;
    }
}
