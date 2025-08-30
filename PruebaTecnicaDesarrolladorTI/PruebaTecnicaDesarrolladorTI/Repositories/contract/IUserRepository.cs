using PruebaTecnicaDesarrolladorTI.Models;

namespace PruebaTecnicaDesarrolladorTI.Repositories.contract
{
    /// <summary>
    /// Contrato para el repositorio de usuarios
    /// Define las operaciones de acceso a datos para la entidad User
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Usuario encontrado o null</returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Usuario encontrado o null</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Verifica si existe un usuario con el username especificado
        /// </summary>
        /// <param name="username">Nombre de usuario a verificar</param>
        /// <returns>True si existe, false en caso contrario</returns>
        Task<bool> ExistsByUsernameAsync(string username);

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="user">Usuario a crear</param>
        /// <returns>Usuario creado con ID asignado</returns>
        Task<User> CreateAsync(User user);

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="user">Usuario con los datos actualizados</param>
        /// <returns>Usuario actualizado</returns>
        Task<User> UpdateAsync(User user);

        /// <summary>
        /// Elimina un usuario (soft delete - marca como inactivo)
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteAsync(int id);
    }
}
