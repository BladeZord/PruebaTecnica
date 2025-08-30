using PruebaTecnicaDesarrolladorTI.Models.DTOs;

namespace PruebaTecnicaDesarrolladorTI.Services.contract
{
    /// <summary>
    /// Contrato para el servicio de productos
    /// Define las operaciones de lógica de negocio para productos
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado o null</returns>
        Task<ProductResponseDto?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        /// <returns>Lista de productos</returns>
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        /// <param name="category">Categoría a filtrar</param>
        /// <returns>Lista de productos de la categoría</returns>
        Task<IEnumerable<ProductResponseDto>> GetByCategoryAsync(string category);

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de productos que coinciden con la búsqueda</returns>
        Task<IEnumerable<ProductResponseDto>> SearchByNameAsync(string searchTerm);

        /// <summary>
        /// Obtiene productos creados por el usuario actual
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de productos del usuario</returns>
        Task<IEnumerable<ProductResponseDto>> GetMyProductsAsync(int userId);

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="productDto">Datos del producto a crear</param>
        /// <param name="userId">ID del usuario que crea el producto</param>
        /// <returns>Producto creado</returns>
        Task<ProductResponseDto> CreateAsync(ProductDto productDto, int userId);

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="id">ID del producto a actualizar</param>
        /// <param name="productDto">Nuevos datos del producto</param>
        /// <param name="userId">ID del usuario que actualiza</param>
        /// <returns>Producto actualizado</returns>
        Task<ProductResponseDto?> UpdateAsync(int id, ProductDto productDto, int userId);

        /// <summary>
        /// Elimina un producto
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <param name="userId">ID del usuario que elimina</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteAsync(int id, int userId);

        /// <summary>
        /// Obtiene estadísticas de productos
        /// </summary>
        /// <returns>Objeto con estadísticas</returns>
        Task<object> GetStatisticsAsync();
    }
}
