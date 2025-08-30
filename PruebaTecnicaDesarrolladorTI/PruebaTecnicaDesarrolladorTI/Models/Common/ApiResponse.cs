namespace PruebaTecnicaDesarrolladorTI.Models.Common
{
    /// <summary>
    /// Clase genérica para respuestas estándar de la API
    /// </summary>
    /// <typeparam name="T">Tipo de datos en la respuesta</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la respuesta
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Datos de la respuesta
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Lista de errores si los hay
        /// </summary>
        public IEnumerable<string>? Errors { get; set; }

        /// <summary>
        /// Código de estado HTTP
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Timestamp de la respuesta
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Constructor para respuesta exitosa
        /// </summary>
        /// <param name="data">Datos de la respuesta</param>
        /// <param name="message">Mensaje opcional</param>
        /// <param name="statusCode">Código de estado</param>
        public ApiResponse(T data, string message = "Operación exitosa", int statusCode = 200)
        {
            Success = true;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Constructor para respuesta de error
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="statusCode">Código de estado</param>
        /// <param name="errors">Lista de errores</param>
        public ApiResponse(string message, int statusCode = 400, IEnumerable<string>? errors = null)
        {
            Success = false;
            Message = message;
            StatusCode = statusCode;
            Errors = errors;
        }

        /// <summary>
        /// Crea una respuesta exitosa
        /// </summary>
        /// <param name="data">Datos de la respuesta</param>
        /// <param name="message">Mensaje opcional</param>
        /// <param name="statusCode">Código de estado</param>
        /// <returns>Respuesta exitosa</returns>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa", int statusCode = 200)
        {
            return new ApiResponse<T>(data, message, statusCode);
        }

        /// <summary>
        /// Crea una respuesta de error
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="statusCode">Código de estado</param>
        /// <param name="errors">Lista de errores</param>
        /// <returns>Respuesta de error</returns>
        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400, IEnumerable<string>? errors = null)
        {
            return new ApiResponse<T>(message, statusCode, errors);
        }
    }

    /// <summary>
    /// Respuesta API sin datos específicos
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        /// <summary>
        /// Constructor para respuesta exitosa sin datos
        /// </summary>
        /// <param name="message">Mensaje de éxito</param>
        /// <param name="statusCode">Código de estado</param>
        public ApiResponse(string message = "Operación exitosa", int statusCode = 200) 
            : base(null!, message, statusCode)
        {
        }

        /// <summary>
        /// Constructor para respuesta de error
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="statusCode">Código de estado</param>
        /// <param name="errors">Lista de errores</param>
        public ApiResponse(string message, int statusCode, IEnumerable<string>? errors = null) 
            : base(message, statusCode, errors)
        {
        }
    }
}
