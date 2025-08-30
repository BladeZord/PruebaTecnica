# PruebaTecnicaDesarrolladorTI

## Descripción

API REST para el manejo de productos con autenticación JWT desarrollada con .NET 8, siguiendo una arquitectura de 3 capas (Controller, Service, Repository) con patrón "contract first".

## Características Implementadas

### 🔐 Autenticación JWT
- **Registro de usuarios** (`POST /api/auth/register`)
- **Login de usuarios** (`POST /api/auth/login`)
- Validación de tokens JWT
- Hash seguro de contraseñas con BCrypt

### 📦 CRUD de Productos
- **Crear producto** (`POST /api/product`) 🔒
- **Listar productos** (`GET /api/product`) 🔒
- **Obtener producto por ID** (`GET /api/product/{id}`) 🔒
- **Actualizar producto** (`PUT /api/product/{id}`) 🔒 *Solo el creador*
- **Eliminar producto** (`DELETE /api/product/{id}`) 🔒 *Solo el creador*
- **Buscar productos** (`GET /api/product/search?searchTerm=...`) 🔒
- **Filtrar por categoría** (`GET /api/product/category/{category}`) 🔒
- **Mis productos** (`GET /api/product/my-products`) 🔒
- **Estadísticas** (`GET /api/product/statistics`) 🔒

*🔒 = Requiere autenticación*

### 🏗️ Arquitectura

#### **3 Capas con Patrón Contract First**

1. **Controllers** (`/Controllers`)
   - `contract/`: Interfaces de controladores
   - `impl/`: Implementaciones de controladores
   - Manejo de respuestas HTTP estándar
   - Validación de modelos

2. **Services** (`/Services`)
   - `contract/`: Interfaces de servicios
   - `impl/`: Implementaciones de servicios
   - Lógica de negocio y validaciones
   - Manejo de excepciones

3. **Repositories** (`/Repositories`)
   - `contract/`: Interfaces de repositorios
   - `impl/`: Implementaciones de repositorios
   - Acceso a datos con Entity Framework
   - `context/`: DbContext de la aplicación

### 💾 Base de Datos
- **MySQL** con Entity Framework Core (Pomelo)
- **Database First** - mapeo a estructura existente
- Tabla `usuario` (id_usuario, usuario, estado, contrasenia)
- Tabla `producto` (id_producto, descripcion, existencia, precio)
- Estado de usuario: 'A'=Activo, 'I'=Inactivo

### 📝 Logging
- **Serilog** con salida a consola y archivos
- Logs estructurados con contexto
- Rotación diaria de archivos
- Diferentes niveles según ambiente

### 📚 Documentación
- **Swagger/OpenAPI** integrado
- Comentarios XML en todo el código
- Documentación automática de endpoints
- Autenticación JWT configurada en Swagger

## 🚀 Instalación y Configuración

### Prerrequisitos
- .NET 8 SDK
- PostgreSQL 12+
- Visual Studio 2022 / VS Code

### 1. Clonar el Repositorio
```bash
git clone <repository-url>
cd PruebaTecnicaDesarrolladorTI
```

### 2. Configurar Base de Datos MySQL
Actualizar la cadena de conexión en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=inventario_db;Uid=app_user;Pwd=AppUser2024!;Port=3306;"
  }
}
```

**Ejecutar el script de base de datos:**
```sql
-- En MySQL Workbench o DBeaver como usuario root:
SOURCE database-scripts/setup-mysql-database.sql;
```

### 3. Restaurar Paquetes
```bash
dotnet restore
```

### 4. Ejecutar la Aplicación
```bash
dotnet run
```

La API estará disponible en:
- **HTTPS**: `https://localhost:7000`
- **HTTP**: `http://localhost:5000`
- **Swagger**: `https://localhost:7000` (raíz)

## 📖 Uso de la API

### 1. Registrar Usuario
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "usuario123",
  "email": "usuario@email.com",
  "password": "MiPassword123",
  "confirmPassword": "MiPassword123"
}
```

### 2. Iniciar Sesión
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "usuario123",
  "password": "MiPassword123"
}
```

**Respuesta:**
```json
{
  "success": true,
  "message": "Login exitoso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2024-01-01T12:00:00Z",
    "user": {
      "id": 1,
      "username": "usuario123",
      "email": "usuario@email.com"
    }
  }
}
```

### 3. Crear Producto
```http
POST /api/product
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Laptop Gaming",
  "description": "Laptop para gaming de alta gama",
  "price": 1299.99,
  "stock": 10,
  "category": "Electrónicos"
}
```

### 4. Listar Productos
```http
GET /api/product?pageNumber=1&pageSize=10
Authorization: Bearer {token}
```

## 🛡️ Seguridad

- **JWT** con HS256 para autenticación
- **BCrypt** para hash de contraseñas
- **HTTPS** en producción
- **Validaciones** en todos los niveles
- **Autorización** basada en propietario para operaciones CUD

## 🏢 Estructura del Proyecto

```
PruebaTecnicaDesarrolladorTI/
├── Controllers/
│   ├── contract/          # Interfaces de controladores
│   └── impl/             # Implementaciones de controladores
├── Services/
│   ├── contract/         # Interfaces de servicios
│   └── impl/            # Implementaciones de servicios
├── Repositories/
│   ├── contract/        # Interfaces de repositorios
│   ├── impl/           # Implementaciones de repositorios
│   └── context/        # DbContext
├── Models/
│   ├── DTOs/          # Data Transfer Objects
│   ├── Common/        # Modelos comunes (ApiResponse)
│   └── Configuration/ # Modelos de configuración
├── logs/             # Archivos de log
├── appsettings.json  # Configuración principal
└── Program.cs        # Punto de entrada
```

## ⚡ Características Técnicas

- **.NET 8** con C# 12
- **Entity Framework Core 8** con MySQL (Pomelo)
- **JWT Bearer Authentication**
- **Serilog** para logging estructurado
- **Swagger/OpenAPI 3.0**
- **Health Checks**
- **CORS** configurado
- **Paginación** en listados
- **Mapeo** a estructura de BD existente
- **Validaciones** con Data Annotations
- **Respuestas** estandarizadas con `ApiResponse<T>`

## 🧪 Testing

### Health Check
```http
GET /health
```

### Endpoints Públicos
- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /health`

### Endpoints Protegidos
Todos los endpoints de `/api/product/*` requieren token JWT válido.

## 📋 Validaciones Implementadas

### Usuario
- Username: 3-50 caracteres, único
- Email: formato válido, único
- Password: 6-100 caracteres
- ConfirmPassword: debe coincidir

### Producto
- Name: requerido, máximo 100 caracteres
- Description: máximo 500 caracteres
- Price: mayor a 0
- Stock: no negativo
- Category: máximo 50 caracteres

## 🔧 Configuración Avanzada

### JWT Settings
```json
{
  "JwtSettings": {
    "SecretKey": "TuClaveSecretaMuySegura32Caracteres",
    "Issuer": "PruebaTecnicaDesarrolladorTI",
    "Audience": "PruebaTecnicaDesarrolladorTI-Users",
    "ExpirationMinutes": 60
  }
}
```

### Logging
Los logs se guardan en `/logs/app-YYYY-MM-DD.log` con rotación diaria.

## 🤝 Contribución

1. Fork el proyecto
2. Crear rama feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit cambios (`git commit -am 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Crear Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.

---

**Desarrollado con ❤️ usando .NET 8 y buenas prácticas de arquitectura**
