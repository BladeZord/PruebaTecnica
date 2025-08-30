using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using PruebaTecnicaDesarrolladorTI.Models;
using PruebaTecnicaDesarrolladorTI.Models.Configuration;
using PruebaTecnicaDesarrolladorTI.Models.DTOs;
using PruebaTecnicaDesarrolladorTI.Repositories.contract;
using PruebaTecnicaDesarrolladorTI.Services.contract;

namespace PruebaTecnicaDesarrolladorTI.Services.impl
{
    /// <summary>
    /// Implementación del servicio de autenticación
    /// Maneja la lógica de negocio para autenticación y autorización
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Constructor del servicio de autenticación
        /// </summary>
        /// <param name="userRepository">Repositorio de usuarios</param>
        /// <param name="jwtSettings">Configuración JWT</param>
        /// <param name="logger">Logger para registrar eventos</param>
        public AuthService(
            IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Autentica un usuario con sus credenciales
        /// </summary>
        /// <param name="loginDto">Credenciales de login</param>
        /// <returns>Respuesta de autenticación con token JWT</returns>
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Intento de login para usuario: {Username}", loginDto.Username);

                // Buscar usuario solo por username (no hay email en la tabla)
                var user = await _userRepository.GetByUsernameAsync(loginDto.Username);

                if (user == null)
                {
                    _logger.LogWarning("Usuario no encontrado: {Username}", loginDto.Username);
                    throw new UnauthorizedAccessException("Credenciales inválidas");
                }

                // Verificar contraseña
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Contraseña incorrecta para usuario: {Username}", loginDto.Username);
                    throw new UnauthorizedAccessException("Credenciales inválidas");
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning("Usuario inactivo intentó hacer login: {Username}", loginDto.Username);
                    throw new UnauthorizedAccessException("Usuario inactivo");
                }

                // Generar token JWT
                var token = GenerateJwtToken(user.Id, user.Username ?? string.Empty);
                var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

                _logger.LogInformation("Login exitoso para usuario: {Username}", loginDto.Username);

                return new AuthResponseDto
                {
                    Token = token,
                    ExpiresAt = expiresAt,
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Username = user.Username ?? string.Empty,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el login para usuario: {Username}", loginDto.Username);
                throw;
            }
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        /// <param name="registerDto">Datos de registro del usuario</param>
        /// <returns>Respuesta de autenticación con token JWT</returns>
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Intento de registro para usuario: {Username}", registerDto.Username);

                // Validar que no exista el username
                if (await _userRepository.ExistsByUsernameAsync(registerDto.Username))
                {
                    _logger.LogWarning("Intento de registro con username existente: {Username}", registerDto.Username);
                    throw new InvalidOperationException("El nombre de usuario ya existe");
                }



                // Crear hash de la contraseña
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, BCrypt.Net.BCrypt.GenerateSalt());

                // Crear usuario
                var user = new User
                {
                    Username = registerDto.Username,
                    PasswordHash = passwordHash,
                    Estado = "A"
                };

                user = await _userRepository.CreateAsync(user);

                // Generar token JWT
                var token = GenerateJwtToken(user.Id, user.Username ?? string.Empty);
                var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

                _logger.LogInformation("Registro exitoso para usuario: {Username}", registerDto.Username);

                return new AuthResponseDto
                {
                    Token = token,
                    ExpiresAt = expiresAt,
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Username = user.Username ?? string.Empty,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro para usuario: {Username}", registerDto.Username);
                throw;
            }
        }

        /// <summary>
        /// Genera un token JWT para un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Token JWT generado</returns>
        public string GenerateJwtToken(int userId, string username)
        {
            try
            {
                _logger.LogInformation("Generando token JWT para usuario: {UserId} - {Username}", userId, username);
                _logger.LogInformation("JWT Settings - Issuer: {Issuer}, Audience: {Audience}, SecretKey length: {SecretKeyLength}",
                    _jwtSettings.Issuer, _jwtSettings.Audience, _jwtSettings.SecretKey?.Length);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var now = DateTime.UtcNow;
                var expires = now.AddMinutes(_jwtSettings.ExpirationMinutes);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };

                _logger.LogInformation("Claims creados: {Claims}", string.Join(", ", claims.Select(c => $"{c.Type}:{c.Value}")));

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: expires,
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("Token JWT generado exitosamente. Longitud: {TokenLength}", tokenString.Length);
                
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar token JWT para usuario: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Valida un token JWT
        /// </summary>
        /// <param name="token">Token a validar</param>
        /// <returns>True si el token es válido</returns>
        public bool ValidateToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene el ID del usuario desde un token JWT
        /// </summary>
        /// <param name="token">Token JWT</param>
        /// <returns>ID del usuario o null si es inválido</returns>
        public int? GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);
                
                var userIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
