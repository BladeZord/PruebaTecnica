using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaDesarrolladorTI.Controllers.contract;
using PruebaTecnicaDesarrolladorTI.Models.Common;
using PruebaTecnicaDesarrolladorTI.Models.DTOs;
using PruebaTecnicaDesarrolladorTI.Services.contract;
using System.Security.Claims;

namespace PruebaTecnicaDesarrolladorTI.Controllers.impl
{
    /// <summary>
    /// Controlador de productos
    /// Maneja los endpoints CRUD para productos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class ProductController : ControllerBase, IProductController
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        /// <summary>
        /// Constructor del controlador de productos
        /// </summary>
        /// <param name="productService">Servicio de productos</param>
        /// <param name="logger">Logger para registrar eventos</param>
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        /// <returns>Lista de productos</returns>
        /// <response code="200">Lista de productos obtenida exitosamente</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), 200)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener todos los productos");

                var products = await _productService.GetAllAsync();
                
                _logger.LogInformation("Se obtuvieron {Count} productos", products.Count());
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado</returns>
        /// <response code="200">Producto encontrado</response>
        /// <response code="404">Producto no encontrado</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener producto con ID: {ProductId}", id);

                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Producto no encontrado con ID: {ProductId}", id);
                    return NotFound(new { message = "Producto no encontrado" });
                }

                _logger.LogInformation("Producto obtenido exitosamente: {ProductId}", id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto con ID: {ProductId}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        /// <param name="category">Categoría a filtrar</param>
        /// <returns>Lista de productos de la categoría</returns>
        /// <response code="200">Productos de la categoría obtenidos exitosamente</response>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), 200)]
        public async Task<IActionResult> GetByCategoryAsync(string category)
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener productos de categoría: {Category}", category);

                var products = await _productService.GetByCategoryAsync(category);
                
                _logger.LogInformation("Se obtuvieron {Count} productos de la categoría {Category}", products.Count(), category);
                return Ok(products);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Categoría inválida: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos por categoría: {Category}", category);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de productos que coinciden con la búsqueda</returns>
        /// <response code="200">Búsqueda realizada exitosamente</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), 200)]
        public async Task<IActionResult> SearchAsync([FromQuery] string searchTerm)
        {
            try
            {
                _logger.LogInformation("Solicitud de búsqueda de productos: {SearchTerm}", searchTerm);

                var products = await _productService.SearchByNameAsync(searchTerm);
                
                _logger.LogInformation("Búsqueda completada. Se encontraron {Count} productos para '{SearchTerm}'", 
                    products.Count(), searchTerm);
                return Ok(products);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Término de búsqueda inválido: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la búsqueda: {SearchTerm}", searchTerm);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene productos del usuario autenticado
        /// </summary>
        /// <returns>Lista de productos del usuario</returns>
        /// <response code="200">Productos del usuario obtenidos exitosamente</response>
        [HttpGet("my-products")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), 200)]
        public async Task<IActionResult> GetMyProductsAsync()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("Solicitud para obtener productos del usuario: {UserId}", userId);

                var products = await _productService.GetMyProductsAsync(userId);
                
                _logger.LogInformation("Se obtuvieron {Count} productos del usuario {UserId}", products.Count(), userId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos del usuario");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="productDto">Datos del producto a crear</param>
        /// <returns>Producto creado</returns>
        /// <response code="201">Producto creado exitosamente</response>
        /// <response code="400">Datos de entrada inválidos</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> CreateAsync([FromBody] ProductDto productDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("Solicitud para crear producto por usuario: {UserId}", userId);

                // Validar modelo
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    
                    _logger.LogWarning("Datos de producto inválidos");
                    return BadRequest(new { errors = errors, message = "Datos de entrada inválidos" });
                }

                var product = await _productService.CreateAsync(productDto, userId);
                
                _logger.LogInformation("Producto creado exitosamente con ID: {ProductId} por usuario: {UserId}", 
                    product.Id, userId);
                return StatusCode(201, product);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Error de validación al crear producto: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Error de operación al crear producto: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="id">ID del producto a actualizar</param>
        /// <param name="productDto">Nuevos datos del producto</param>
        /// <returns>Producto actualizado</returns>
        /// <response code="200">Producto actualizado exitosamente</response>
        /// <response code="400">Datos de entrada inválidos</response>
        /// <response code="403">Sin permisos para actualizar el producto</response>
        /// <response code="404">Producto no encontrado</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDto), 200)]
        [ProducesResponseType(typeof(ProductResponseDto), 400)]
        [ProducesResponseType(typeof(ProductResponseDto), 403)]
        [ProducesResponseType(typeof(ProductResponseDto), 404)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductDto productDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("Solicitud para actualizar producto {ProductId} por usuario: {UserId}", id, userId);

                // Validar modelo
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    
                    _logger.LogWarning("Datos de producto inválidos para actualización");
                    return BadRequest(new { errors = errors, message = "Datos de entrada inválidos" });
                }

                var product = await _productService.UpdateAsync(id, productDto, userId);
                if (product == null)
                {
                    _logger.LogWarning("Producto no encontrado para actualizar: {ProductId}", id);
                    return NotFound(new { message = "Producto no encontrado" });
                }

                _logger.LogInformation("Producto actualizado exitosamente: {ProductId} por usuario: {UserId}", id, userId);
                return Ok(product);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Sin permisos para actualizar producto {ProductId}: {Message}", id, ex.Message);
                return StatusCode(403, new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Error de validación al actualizar producto: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto: {ProductId}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        /// <response code="200">Producto eliminado exitosamente</response>
        /// <response code="403">Sin permisos para eliminar el producto</response>
        /// <response code="404">Producto no encontrado</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("Solicitud para eliminar producto {ProductId} por usuario: {UserId}", id, userId);

                var result = await _productService.DeleteAsync(id, userId);
                if (!result)
                {
                    _logger.LogWarning("Producto no encontrado para eliminar: {ProductId}", id);
                    return NotFound(new { message = "Producto no encontrado" });
                }

                _logger.LogInformation("Producto eliminado exitosamente: {ProductId} por usuario: {UserId}", id, userId);
                return Ok(new { message = "Producto eliminado exitosamente" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Sin permisos para eliminar producto {ProductId}: {Message}", id, ex.Message);
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto: {ProductId}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene estadísticas de productos
        /// </summary>
        /// <returns>Estadísticas de productos</returns>
        /// <response code="200">Estadísticas obtenidas exitosamente</response>
        [HttpGet("statistics")]
        [ProducesResponseType(typeof(List<ProductResponseDto>), 200)]
        [ProducesResponseType(typeof(List<ProductResponseDto>), 200)]
        [ProducesResponseType(typeof(List<ProductResponseDto>), 403)]
        [ProducesResponseType(typeof(List<ProductResponseDto>), 404)]
        public async Task<IActionResult> GetStatisticsAsync()
        {
            try
            {
                _logger.LogInformation("Solicitud para obtener estadísticas de productos");

                var statistics = await _productService.GetStatisticsAsync();
                
                _logger.LogInformation("Estadísticas obtenidas exitosamente");
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene el ID del usuario actual desde el token JWT
        /// </summary>
        /// <returns>ID del usuario actual</returns>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Token de usuario inválido");
            }
            return userId;
        }
    }
}
