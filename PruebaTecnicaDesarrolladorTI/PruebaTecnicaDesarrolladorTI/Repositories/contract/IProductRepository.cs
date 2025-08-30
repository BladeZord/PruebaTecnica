using PruebaTecnicaDesarrolladorTI.Models;

namespace PruebaTecnicaDesarrolladorTI.Repositories.contract
{
    /// <summary>
    /// Contrato para el repositorio de productos
    /// Define las operaciones de acceso a datos para la entidad Product
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado o null</returns>
        Task<Product?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los productos activos
        /// </summary>
        /// <returns>Lista de productos</returns>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        /// <param name="category">Categoría a filtrar</param>
        /// <returns>Lista de productos de la categoría</returns>
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de productos que coinciden con la búsqueda</returns>
        Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm);

        /// <summary>
        /// Obtiene productos creados por un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario creador</param>
        /// <returns>Lista de productos del usuario</returns>
        Task<IEnumerable<Product>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Obtiene el número total de productos activos
        /// </summary>
        /// <returns>Cantidad total de productos</returns>
        Task<int> GetTotalCountAsync();

        /// <summary>
        /// Obtiene el número total de productos por categoría
        /// </summary>
        /// <param name="category">Categoría a contar</param>
        /// <returns>Cantidad de productos en la categoría</returns>
        Task<int> GetCountByCategoryAsync(string category);

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="product">Producto a crear</param>
        /// <returns>Producto creado con ID asignado</returns>
        Task<Product> CreateAsync(Product product);

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="product">Producto con los datos actualizados</param>
        /// <returns>Producto actualizado</returns>
        Task<Product> UpdateAsync(Product product);

        /// <summary>
        /// Elimina un producto (soft delete - marca como inactivo)
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Verifica si un producto existe y está activo
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>True si existe y está activo</returns>
        Task<bool> ExistsAsync(int id);
    }
}
