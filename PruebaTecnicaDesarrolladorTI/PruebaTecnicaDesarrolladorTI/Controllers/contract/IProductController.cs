using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaDesarrolladorTI.Models.Common;
using PruebaTecnicaDesarrolladorTI.Models.DTOs;

namespace PruebaTecnicaDesarrolladorTI.Controllers.contract
{
    /// <summary>
    /// Contrato para el controlador de productos
    /// Define los endpoints CRUD para productos
    /// </summary>
    public interface IProductController
    {
        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        /// <returns>Lista de productos</returns>
        Task<IActionResult> GetAllAsync();

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado</returns>
        Task<IActionResult> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        /// <param name="category">Categoría a filtrar</param>
        /// <returns>Lista de productos de la categoría</returns>
        Task<IActionResult> GetByCategoryAsync(string category);

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de productos que coinciden con la búsqueda</returns>
        Task<IActionResult> SearchAsync(string searchTerm);

        /// <summary>
        /// Obtiene productos del usuario autenticado
        /// </summary>
        /// <returns>Lista de productos del usuario</returns>
        Task<IActionResult> GetMyProductsAsync();

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="productDto">Datos del producto a crear</param>
        /// <returns>Producto creado</returns>
        Task<IActionResult> CreateAsync(ProductDto productDto);

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="id">ID del producto a actualizar</param>
        /// <param name="productDto">Nuevos datos del producto</param>
        /// <returns>Producto actualizado</returns>
        Task<IActionResult> UpdateAsync(int id, ProductDto productDto);

        /// <summary>
        /// Elimina un producto
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        Task<IActionResult> DeleteAsync(int id);

        /// <summary>
        /// Obtiene estadísticas de productos
        /// </summary>
        /// <returns>Estadísticas de productos</returns>
        Task<IActionResult> GetStatisticsAsync();
    }
}
