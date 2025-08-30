using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaDesarrolladorTI.Models.Common;
using PruebaTecnicaDesarrolladorTI.Models.DTOs;

namespace PruebaTecnicaDesarrolladorTI.Controllers.contract
{
    /// <summary>
    /// Contrato para el controlador de autenticación
    /// Define los endpoints de autenticación
    /// </summary>
    public interface IAuthController
    {
        /// <summary>
        /// Endpoint para el login de usuarios
        /// </summary>
        /// <param name="loginDto">Credenciales de login</param>
        /// <returns>Respuesta con token de autenticación</returns>
        Task<IActionResult> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Endpoint para el registro de nuevos usuarios
        /// </summary>
        /// <param name="registerDto">Datos de registro</param>
        /// <returns>Respuesta con token de autenticación</returns>
        Task<IActionResult> RegisterAsync(RegisterDto registerDto);
    }
}
