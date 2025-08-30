using PruebaTecnicaDesarrolladorTI.Models;
using PruebaTecnicaDesarrolladorTI.Models.DTOs;
using PruebaTecnicaDesarrolladorTI.Repositories.contract;
using PruebaTecnicaDesarrolladorTI.Services.contract;

namespace PruebaTecnicaDesarrolladorTI.Services.impl
{
    /// <summary>
    /// Implementación del servicio de productos
    /// Maneja la lógica de negocio para productos
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ProductService> _logger;

        /// <summary>
        /// Constructor del servicio de productos
        /// </summary>
        /// <param name="productRepository">Repositorio de productos</param>
        /// <param name="userRepository">Repositorio de usuarios</param>
        /// <param name="logger">Logger para registrar eventos</param>
        public ProductService(
            IProductRepository productRepository,
            IUserRepository userRepository,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado o null</returns>
        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo producto con ID: {ProductId}", id);

                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Producto no encontrado con ID: {ProductId}", id);
                    return null;
                }

                return MapToResponseDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto con ID: {ProductId}", id);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        /// <returns>Lista de productos</returns>
        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los productos");

                var products = await _productRepository.GetAllAsync();
                return products.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                throw;
            }
        }

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        /// <param name="category">Categoría a filtrar</param>
        /// <returns>Lista de productos de la categoría</returns>
        public async Task<IEnumerable<ProductResponseDto>> GetByCategoryAsync(string category)
        {
            try
            {
                _logger.LogInformation("Obteniendo productos por categoría: {Category}", category);

                if (string.IsNullOrWhiteSpace(category))
                    throw new ArgumentException("La categoría no puede estar vacía");

                var products = await _productRepository.GetByCategoryAsync(category);
                return products.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos por categoría: {Category}", category);
                throw;
            }
        }

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de productos que coinciden con la búsqueda</returns>
        public async Task<IEnumerable<ProductResponseDto>> SearchByNameAsync(string searchTerm)
        {
            try
            {
                _logger.LogInformation("Buscando productos por nombre: {SearchTerm}", searchTerm);

                if (string.IsNullOrWhiteSpace(searchTerm))
                    throw new ArgumentException("El término de búsqueda no puede estar vacío");

                var products = await _productRepository.SearchByNameAsync(searchTerm);
                return products.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar productos por nombre: {SearchTerm}", searchTerm);
                throw;
            }
        }

        /// <summary>
        /// Obtiene productos creados por el usuario actual
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de productos del usuario</returns>
        public async Task<IEnumerable<ProductResponseDto>> GetMyProductsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Obteniendo productos del usuario: {UserId}", userId);

                var products = await _productRepository.GetByUserIdAsync(userId);
                return products.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos del usuario: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="productDto">Datos del producto a crear</param>
        /// <param name="userId">ID del usuario que crea el producto</param>
        /// <returns>Producto creado</returns>
        public async Task<ProductResponseDto> CreateAsync(ProductDto productDto, int userId)
        {
            try
            {
                _logger.LogInformation("Creando producto para usuario: {UserId}", userId);

                // Validar que el usuario existe
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new InvalidOperationException("Usuario no encontrado");

                // Validar datos del producto
                ValidateProductData(productDto);

                var product = new Product
                {
                   
                    Description = productDto.Description?.Trim() ?? string.Empty,
                    Price = (double)productDto.Price,
                    Stock = productDto.Stock,
                };

                product = await _productRepository.CreateAsync(product);
                _logger.LogInformation("Producto creado exitosamente con ID: {ProductId}", product.Id);

                return MapToResponseDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto para usuario: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="id">ID del producto a actualizar</param>
        /// <param name="productDto">Nuevos datos del producto</param>
        /// <param name="userId">ID del usuario que actualiza</param>
        /// <returns>Producto actualizado</returns>
        public async Task<ProductResponseDto?> UpdateAsync(int id, ProductDto productDto, int userId)
        {
            try
            {
                _logger.LogInformation("Actualizando producto {ProductId} por usuario: {UserId}", id, userId);

                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Producto no encontrado para actualizar: {ProductId}", id);
                    return null;
                }
                // Validar datos del producto
                ValidateProductData(productDto);

                // Actualizar campos
                existingProduct.Description = productDto.Description?.Trim() ?? string.Empty;
                existingProduct.Price = (double)productDto.Price;
                existingProduct.Stock = productDto.Stock;

                var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
                _logger.LogInformation("Producto actualizado exitosamente: {ProductId}", id);

                return MapToResponseDto(updatedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto {ProductId} por usuario: {UserId}", id, userId);
                throw;
            }
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <param name="userId">ID del usuario que elimina</param>
        /// <returns>True si se eliminó correctamente</returns>
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            try
            {
                _logger.LogInformation("Eliminando producto {ProductId} por usuario: {UserId}", id, userId);

                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Producto no encontrado para eliminar: {ProductId}", id);
                    return false;
                }


                var result = await _productRepository.DeleteAsync(id);
                if (result)
                {
                    _logger.LogInformation("Producto eliminado exitosamente: {ProductId}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto {ProductId} por usuario: {UserId}", id, userId);
                throw;
            }
        }

        /// <summary>
        /// Obtiene estadísticas de productos
        /// </summary>
        /// <returns>Objeto con estadísticas</returns>
        public async Task<object> GetStatisticsAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo estadísticas de productos");

                var totalProducts = await _productRepository.GetTotalCountAsync();

                return new
                {
                    TotalProducts = totalProducts,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de productos");
                throw;
            }
        }

        /// <summary>
        /// Valida los datos de un producto
        /// </summary>
        /// <param name="productDto">Datos del producto a validar</param>
        private static void ValidateProductData(ProductDto productDto)
        {

            if (!string.IsNullOrEmpty(productDto.Description) && productDto.Description.Length > 500)
                throw new ArgumentException("La descripción no puede exceder 500 caracteres");

            if (productDto.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");

            if (productDto.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo");
        }

        /// <summary>
        /// Mapea un producto a ProductResponseDto
        /// </summary>
        /// <param name="product">Producto a mapear</param>
        /// <returns>ProductResponseDto</returns>
        private static ProductResponseDto MapToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Description = product.Description ?? string.Empty,
                Price = (decimal)(product.Price ?? 0),
                Stock = product.Stock ?? 0
            };
        }
    }
}
