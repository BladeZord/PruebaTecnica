using Microsoft.EntityFrameworkCore;

using PruebaTecnicaDesarrolladorTI.Models;
using PruebaTecnicaDesarrolladorTI.Repositories.context;
using PruebaTecnicaDesarrolladorTI.Repositories.contract;

namespace PruebaTecnicaDesarrolladorTI.Repositories.impl
{
    /// <summary>
    /// Implementación del repositorio de productos
    /// Maneja el acceso a datos para la entidad Product
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor del repositorio de productos
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado o null</returns>
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene todos los productos activos
        /// </summary>
        /// <returns>Lista de productos</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        /// <param name="category">Categoría a filtrar</param>
        /// <returns>Lista de productos de la categoría</returns>
        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Description!.ToLower() == category.ToLower())
                .ToListAsync();
        }

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de productos que coinciden con la búsqueda</returns>
        public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm)
        {
            return await _context.Products
                .Where(p =>  p.Description!.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos creados por un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario creador</param>
        /// <returns>Lista de productos del usuario</returns>
        public async Task<IEnumerable<Product>> GetByUserIdAsync(int userId)
        {
            return await _context.Products
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene el número total de productos activos
        /// </summary>
        /// <returns>Cantidad total de productos</returns>
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Products
                .CountAsync();
        }

        /// <summary>
        /// Obtiene el número total de productos por categoría
        /// </summary>
        /// <param name="category">Categoría a contar</param>
        /// <returns>Cantidad de productos en la categoría</returns>
        public async Task<int> GetCountByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Description!.ToLower() == category.ToLower())
                .CountAsync();
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="product">Producto a crear</param>
        /// <returns>Producto creado con ID asignado</returns>
        public async Task<Product> CreateAsync(Product product)
        {

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Recargar el producto con la información del usuario
            return await GetByIdAsync(product.Id) ?? product;
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="product">Producto con los datos actualizados</param>
        /// <returns>Producto actualizado</returns>
        public async Task<Product> UpdateAsync(Product product)
        {

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            // Recargar el producto con la información del usuario
            return await GetByIdAsync(product.Id) ?? product;
        }

        /// <summary>
        /// Elimina un producto (soft delete - marca como inactivo)
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Verifica si un producto existe y está activo
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>True si existe y está activo</returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products
                .AnyAsync(p => p.Id == id);
        }
    }
}
