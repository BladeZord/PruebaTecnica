using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaDesarrolladorTI.Controllers.contract;
using PruebaTecnicaDesarrolladorTI.Models.Common;
using PruebaTecnicaDesarrolladorTI.Models.DTOs;
using PruebaTecnicaDesarrolladorTI.Services.contract;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaDesarrolladorTI.Controllers.impl
{
    /// <summary>
    /// Controlador de autenticación
    /// Maneja los endpoints de login y registro
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Constructor del controlador de autenticación
        /// </summary>
        /// <param name="authService">Servicio de autenticación</param>
        /// <param name="logger">Logger para registrar eventos</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint para el login de usuarios
        /// </summary>
        /// <param name="loginDto">Credenciales de login</param>
        /// <returns>Respuesta con token de autenticación</returns>
        /// <response code="200">Login exitoso, retorna token JWT</response>
        /// <response code="400">Datos de entrada inválidos</response>
        /// <response code="401">Credenciales incorrectas</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Solicitud de login recibida para usuario: {Username}", loginDto.Username);

                // Validar modelo
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    
                    _logger.LogWarning("Datos de login inválidos para usuario: {Username}", loginDto.Username);
                    return BadRequest(new { errors = errors, message = "Datos de entrada inválidos" });
                }

                var result = await _authService.LoginAsync(loginDto);
                
                _logger.LogInformation("Login exitoso para usuario: {Username}", loginDto.Username);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Login fallido para usuario {Username}: {Message}", loginDto.Username, ex.Message);
                return Unauthorized(new { message = "Credenciales inválidas" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno durante login para usuario: {Username}", loginDto.Username);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Endpoint para el registro de nuevos usuarios
        /// </summary>
        /// <param name="registerDto">Datos de registro</param>
        /// <returns>Respuesta con token de autenticación</returns>
        /// <response code="201">Registro exitoso, retorna token JWT</response>
        /// <response code="400">Datos de entrada inválidos o usuario ya existe</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Solicitud de registro recibida para usuario: {Username}", registerDto.Username);

                // Validar modelo
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    
                    _logger.LogWarning("Datos de registro inválidos para usuario: {Username}", registerDto.Username);
                    return BadRequest(new { errors = errors, message = "Datos de entrada inválidos" });
                }

                var result = await _authService.RegisterAsync(registerDto);
                
                _logger.LogInformation("Registro exitoso para usuario: {Username}", registerDto.Username);
                return StatusCode(201, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Registro fallido para usuario {Username}: {Message}", registerDto.Username, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno durante registro para usuario: {Username}", registerDto.Username);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}
