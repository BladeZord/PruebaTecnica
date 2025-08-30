using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PruebaTecnicaDesarrolladorTI.Models.Configuration;
using PruebaTecnicaDesarrolladorTI.Repositories.context;
using PruebaTecnicaDesarrolladorTI.Repositories.contract;
using PruebaTecnicaDesarrolladorTI.Repositories.impl;
using PruebaTecnicaDesarrolladorTI.Services.contract;
using PruebaTecnicaDesarrolladorTI.Services.impl;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Iniciando aplicación PruebaTecnicaDesarrolladorTI");

    // Configurar servicios
    ConfigureServices(builder.Services, builder.Configuration);

    var app = builder.Build();

    // Configurar pipeline de la aplicación
    await ConfigureApplication(app);

    Log.Information("Aplicación configurada exitosamente");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error fatal durante el inicio de la aplicación");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

/// <summary>
/// Configura todos los servicios de la aplicación
/// </summary>
/// <param name="services">Colección de servicios</param>
/// <param name="configuration">Configuración de la aplicación</param>
static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Configurar controladores
    services.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = false;
        });

    // Configurar Entity Framework con MySQL
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));

    // Configurar JWT
    var jwtSettings = configuration.GetSection("JwtSettings");
    services.Configure<JwtSettings>(jwtSettings);

    var secretKey = jwtSettings.GetValue<string>("SecretKey");
    var issuer = jwtSettings.GetValue<string>("Issuer");
    var audience = jwtSettings.GetValue<string>("Audience");
    
    // Debug JWT configuration
    Console.WriteLine($"JWT Config - Issuer: {issuer}, Audience: {audience}, SecretKey length: {secretKey?.Length}");

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            // Eventos para debugging JWT
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                    Console.WriteLine($"Exception type: {context.Exception.GetType().Name}");
                    if (context.Exception.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {context.Exception.InnerException.Message}");
                        Console.WriteLine($"Inner exception type: {context.Exception.InnerException.GetType().Name}");
                    }
                    Console.WriteLine($"Stack trace: {context.Exception.StackTrace}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("JWT Token validated successfully");
                    var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
                    Console.WriteLine($"Claims: {string.Join(", ", claims ?? new string[0])}");
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
                    Console.WriteLine($"Authorization header: {authHeader}");
                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    {
                        var token = authHeader.Substring("Bearer ".Length);
                        Console.WriteLine($"JWT Token received: {token[..Math.Min(20, token.Length)]}...");
                        Console.WriteLine($"Token length: {token.Length}");
                        
                        // Validar que el token tenga el formato correcto
                        var parts = token.Split('.');
                        Console.WriteLine($"Token parts: {parts.Length} (should be 3)");
                        if (parts.Length == 3)
                        {
                            Console.WriteLine($"Header part length: {parts[0].Length}");
                            Console.WriteLine($"Payload part length: {parts[1].Length}");
                            Console.WriteLine($"Signature part length: {parts[2].Length}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Bearer token found in Authorization header");
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    Console.WriteLine($"JWT Challenge - Error: '{context.Error}', ErrorDescription: '{context.ErrorDescription}'");
                    Console.WriteLine($"JWT Challenge - AuthenticateFailure: '{context.AuthenticateFailure?.Message}'");
                    Console.WriteLine($"JWT Challenge - HttpContext.User.Identity.IsAuthenticated: {context.HttpContext.User.Identity?.IsAuthenticated}");
                    return Task.CompletedTask;
                }
            };
        });

    services.AddAuthorization();

    // Registrar repositorios
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IProductRepository, ProductRepository>();

    // Registrar servicios
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IProductService, ProductService>();

    // Configurar Swagger/OpenAPI
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "PruebaTecnicaDesarrolladorTI API",
            Version = "v1",
            Description = "API para el manejo de productos con autenticación JWT",
            Contact = new OpenApiContact
            {
                Name = "Kevin Quito",
                Email = "kevinquito100@gmail.com"
            }
        });

        // Configurar autenticación JWT en Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });

        // Incluir comentarios XML
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });

    // Configurar CORS
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Configurar Health Checks
    services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();
}

/// <summary>
/// Configura el pipeline de la aplicación
/// </summary>
/// <param name="app">Aplicación web</param>
static async Task ConfigureApplication(WebApplication app)
{
    // Configurar manejo de excepciones
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PruebaTecnicaDesarrolladorTI API V1");
        });
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Configurar pipeline HTTP
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    
    // Middleware de logging de requests
    app.UseSerilogRequestLogging();

    // Middleware de autenticación y autorización
    app.UseAuthentication();
    app.UseAuthorization();

    // Mapear controladores
    app.MapControllers();

    // Mapear Health Checks
    app.MapHealthChecks("/health");

    // Asegurar que la base de datos existe
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
            Log.Information("Base de datos configurada exitosamente");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error al configurar la base de datos");
            throw;
        }
    }
}
