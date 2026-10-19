# WebAPISolution — ASP.NET Core Web API REST + EF Core + SQL Server

Recreación del proyecto del video "Aprende a crear una ASP.NET Core Web API REST desde cero con C#, Entity Framework Core y SQL Server".
CRUD de **Estudiantes, Cursos, Docentes y Asignaciones** sobre la base **University** en SQL Server LocalDB.

## Estructura
```
WebAPISolution.sln
WebAPISolution/
├── Contexts/ApplicationDbContext.cs      ← DbContext con DbSet<Estudiante> Estudiante
├── Controllers/EstudiantesController.cs  ← CRUD estudiantes (scaffold de EF, del video)
├── Controllers/CursosController.cs       ← CRUD cursos
├── Controllers/DocentesController.cs     ← CRUD docentes
├── Controllers/AsignacionesController.cs ← CRUD asignaciones (con Include y validación de FKs)
├── Controllers/WeatherForecastController.cs (de la plantilla)
├── Migrations/                           ← InitialSetupDb + AgregarCursoDocenteAsignacion
├── Models/                               ← Estudiante, Curso, Docente, Asignacion
├── Properties/launchSettings.json        ← perfil IIS Express en https://localhost:44322
├── appsettings.json                      ← cadena de conexión DefaultConnection
├── Program.cs                            ← registro de AddDbContext + MapGet("/")
├── WebAPISolution.http                   ← peticiones de prueba (como en Postman)
└── WebAPISolution.csproj
postman/WebAPISolution.postman_collection.json
```

## Pasos que se siguen en el video
1. **Visual Studio 2026 → Create a new project → ASP.NET Core Web API (C#)**. Nombre `WebAPISolution`.
   Framework **.NET 10.0 (LTS)**, Authentication *None*, ✔ Configure for HTTPS, ✔ Enable OpenAPI support, ✔ Use controllers.
2. Crear las carpetas **Models** y **Contexts**.
3. En *Models* agregar la clase **Estudiante**: `[Key] EstudianteId`, y `Nombres` / `Apellidos` con
   `[Required(ErrorMessage = "Este campo es requerido")]` y `[StringLength(50)]`.
4. En *Contexts* agregar **ApplicationDbContext : DbContext** (constructor con `DbContextOptions<ApplicationDbContext>`,
   constructor protegido vacío, `OnModelCreating` y `DbSet<Estudiante> Estudiante`).
5. Instalar paquetes NuGet (Tools → NuGet Package Manager → Manage NuGet Packages for Solution):
   `Microsoft.EntityFrameworkCore`, `.SqlServer`, `.Tools`, `.Design` (versión 10.0.x).
   En el video también se actualiza `System.Security.Cryptography.Pkcs` porque NuGet marcaba una vulnerabilidad.
6. En **appsettings.json** agregar `ConnectionStrings:DefaultConnection`
   → `server=(localdb)\\MSSQLLocalDB;database=University;trusted_connection=true;`
7. En **Program.cs** registrar el contexto:
   `builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));`
8. Compilar (aparecen advertencias CS8618 de "non-nullable", se ignoran igual que en el video).
9. **Package Manager Console**:
   ```
   Add-Migration InitialSetupDb
   Update-Database
   ```
10. Verificar en **SQL Server Management Studio** (`(localdb)\MSSQLLocalDB`): base `University`, tabla `dbo.Estudiante`
    y `__EFMigrationsHistory`. Consulta: `use University; select * from Estudiante`.
11. Clic derecho en *Controllers* → Add → New Scaffolded Item → **API Controller with actions, using Entity Framework**.
    Model: `Estudiante`, DbContext: `ApplicationDbContext`, provider SQL Server, nombre `EstudiantesController`.
12. Ejecutar con **IIS Express**. La raíz daba 404, así que se agrega en Program.cs
    `app.MapGet("/", () => "Nuestra Web API está funcionando perfectamente....");`
13. Probar `https://localhost:44322/api/estudiantes` en el navegador (devuelve `[]`) y luego en **Postman**:
    - `POST /api/estudiantes` con body raw JSON `{"nombres":"Armando","apellidos":"Cajas"}` → **201 Created**
    - `POST` con `{"nombres":"Juan","apellidos":"Pérez"}` → 201
    - `GET /api/estudiantes` → lista con los 2 estudiantes (200 OK)
14. Repaso del controlador: ruta `api/[controller]`, inyección de `ApplicationDbContext`, y los métodos
    GET (todos / por id), PUT, POST (`CreatedAtAction`), DELETE y `EstudianteExists`.

## Cómo ejecutarlo
- Abrir `WebAPISolution.sln` en Visual Studio 2026 (o `dotnet restore`).
- Requisitos: .NET 10 SDK y SQL Server LocalDB (viene con Visual Studio, carga "ASP.NET y desarrollo web").
- La migración ya está incluida, solo falta crear la base: `Update-Database` (PMC) o `dotnet ef database update`.
  Si prefieres generarla tú como en el video, borra la carpeta *Migrations* y ejecuta `Add-Migration InitialSetupDb`.
- Ejecutar con el perfil **IIS Express** (https://localhost:44322) o `https` (https://localhost:7018).
- Probar con `WebAPISolution.http` (Visual Studio lo abre directo) o importando la colección de Postman.

## Ampliación según el diagrama de clases
```
Curso (CursoId, Nombre)            1 ──< N  Asignacion
Docente (DocenteId, Nombres, Apellidos) 1 ──< N  Asignacion
Estudiante (EstudianteId, Nombres, Apellidos, Direccion) 1 ──< N  Asignacion
Asignacion (AsignacionId, EstudianteId FK, CursoId FK, DocenteId FK)
```
- `Estudiante` gana el campo `Direccion` (opcional, máx. 100) para no romper los registros que ya existen.
- `Asignacion` tiene propiedades de navegación (`Estudiante`, `Curso`, `Docente`) configuradas en `OnModelCreating`
  con `HasOne(...).WithMany(...).HasForeignKey(...)`. Las colecciones `Asignaciones` llevan `[JsonIgnore]` para evitar ciclos en el JSON.
- `GET /api/asignaciones` devuelve cada asignación con los datos del estudiante, curso y docente (`Include`).
- `POST`/`PUT` de asignaciones responden **400** con un mensaje claro si el estudiante, curso o docente no existe.
- Migración nueva: `AgregarCursoDocenteAsignacion` (ya incluida). Para aplicarla: `Update-Database`.
  Si la quieres generar tú: borra sus dos archivos, restaura el snapshot anterior y ejecuta `Add-Migration AgregarCursoDocenteAsignacion`.
- Orden de prueba: crear estudiante → curso → docente → asignación.

## Endpoints
| Método | Ruta | Respuesta |
|---|---|---|
| GET | `/` | Texto de prueba |
| GET | `/api/estudiantes` | 200 + lista |
| GET | `/api/estudiantes/{id}` | 200 / 404 |
| POST | `/api/estudiantes` | 201 Created |
| PUT | `/api/estudiantes/{id}` | 204 / 400 / 404 |
| DELETE | `/api/estudiantes/{id}` | 204 / 404 |
| GET/POST | `/api/cursos` | igual que estudiantes |
| GET/PUT/DELETE | `/api/cursos/{id}` | |
| GET/POST | `/api/docentes` | igual que estudiantes |
| GET/PUT/DELETE | `/api/docentes/{id}` | |
| GET/POST | `/api/asignaciones` | GET incluye estudiante, curso y docente; POST valida FKs (400) |
| GET/PUT/DELETE | `/api/asignaciones/{id}` | |
