using PruebaTecnicaDesarrolladorTI.Models.DTOs;

namespace PruebaTecnicaDesarrolladorTI.Services.contract
{
    /// <summary>
    /// Contrato para el servicio de autenticación
    /// Define las operaciones de autenticación y autorización
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica un usuario con sus credenciales
        /// </summary>
        /// <param name="loginDto">Credenciales de login</param>
        /// <returns>Respuesta de autenticación con token JWT</returns>
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        /// <param name="registerDto">Datos de registro del usuario</param>
        /// <returns>Respuesta de autenticación con token JWT</returns>
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// Genera un token JWT para un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Token JWT generado</returns>
        string GenerateJwtToken(int userId, string username);

        /// <summary>
        /// Valida un token JWT
        /// </summary>
        /// <param name="token">Token a validar</param>
        /// <returns>True si el token es válido</returns>
        bool ValidateToken(string token);

        /// <summary>
        /// Obtiene el ID del usuario desde un token JWT
        /// </summary>
        /// <param name="token">Token JWT</param>
        /// <returns>ID del usuario o null si es inválido</returns>
        int? GetUserIdFromToken(string token);
    }
}
