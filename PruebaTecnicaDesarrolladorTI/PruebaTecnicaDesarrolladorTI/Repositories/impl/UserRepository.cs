using Microsoft.EntityFrameworkCore;

using PruebaTecnicaDesarrolladorTI.Constans;
using PruebaTecnicaDesarrolladorTI.Models;
using PruebaTecnicaDesarrolladorTI.Repositories.context;
using PruebaTecnicaDesarrolladorTI.Repositories.contract;

namespace PruebaTecnicaDesarrolladorTI.Repositories.impl
{
    /// <summary>
    /// Implementación del repositorio de usuarios
    /// Maneja el acceso a datos para la entidad User
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor del repositorio de usuarios
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Usuario encontrado o null</returns>
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id && u.Estado == "A")
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Usuario encontrado o null</returns>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Where(u => u.Username == username && u.Estado == Estados.ACTIVO)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Verifica si existe un usuario con el username especificado
        /// </summary>
        /// <param name="username">Nombre de usuario a verificar</param>
        /// <returns>True si existe, false en caso contrario</returns>
        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username && u.Estado == Estados.ACTIVO);
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="user">Usuario a crear</param>
        /// <returns>Usuario creado con ID asignado</returns>
        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="user">Usuario con los datos actualizados</param>
        /// <returns>Usuario actualizado</returns>
        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            return user;
        }

        /// <summary>
        /// Elimina un usuario (soft delete - marca como inactivo)
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
                return false;

            user.Estado = "I"; // Inactivo
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
