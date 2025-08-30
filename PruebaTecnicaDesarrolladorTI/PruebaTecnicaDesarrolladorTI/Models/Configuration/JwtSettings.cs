namespace PruebaTecnicaDesarrolladorTI.Models.Configuration
{
    /// <summary>
    /// Configuración para JWT (JSON Web Tokens)
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Clave secreta para firmar los tokens
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Emisor del token (issuer)
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Audiencia del token (audience)
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Tiempo de expiración del token en minutos
        /// </summary>
        public int ExpirationMinutes { get; set; } = 60;

        /// <summary>
        /// Tiempo de expiración del refresh token en días
        /// </summary>
        public int RefreshExpirationDays { get; set; } = 7;
    }
}
