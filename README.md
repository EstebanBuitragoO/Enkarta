# Enkarta

Aplicación de escritorio WPF para gestión de artículos, medios, autores y contenido relacionado.

## Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior
- [XAMPP](https://www.apachefriends.org/) o cualquier servidor MySQL
- Visual Studio 2022 o Visual Studio Code (opcional)
- Git

## Tecnologías Utilizadas

- **.NET 8.0** - Framework de aplicación
- **WPF (Windows Presentation Foundation)** - Framework de UI
- **Entity Framework Core 9.0** - ORM para acceso a datos
- **MySQL** - Base de datos
- **Pomelo.EntityFrameworkCore.MySql** - Proveedor de MySQL para EF Core

## Instalación

### 1. Clonar el Repositorio

```bash
git clone <url-del-repositorio>
cd Enkarta/Enkarta
```

### 2. Restaurar Dependencias

```bash
dotnet restore
```

### 3. Configurar la Base de Datos

#### a) Iniciar MySQL

Si usas XAMPP:
1. Abre el panel de control de XAMPP
2. Inicia el servicio **MySQL**

#### b) Configurar la Cadena de Conexión

Edita el archivo `Models/Conex/DbConex.cs` en la línea 35 si tu configuración de MySQL es diferente:

```csharp
// Configuración por defecto de XAMPP (sin password)
var connectionString = "server=localhost;database=enkarta;user=root;password=";
```

Si tienes contraseña en MySQL, agrégala:

```csharp
var connectionString = "server=localhost;database=enkarta;user=root;password=tu_password";
```

#### c) Instalar Herramientas de EF Core (Solo la primera vez)

```bash
dotnet tool install --global dotnet-ef
```

O si ya la tienes instalada, actualízala:

```bash
dotnet tool update --global dotnet-ef
```

#### d) Aplicar Migraciones

La base de datos y las tablas se crearán automáticamente:

```bash
dotnet ef database update
```

Esto creará:
- Base de datos `enkarta`
- Todas las tablas con sus relaciones
- Índices y foreign keys

### 4. Compilar el Proyecto

```bash
dotnet build
```

### 5. Ejecutar la Aplicación

```bash
dotnet run
```

## Estructura del Proyecto

```
Enkarta/
├── Models/                      # Modelos de datos (entidades)
│   ├── Conex/                   # Contexto de base de datos
│   │   ├── DbConex.cs          # DbContext de Entity Framework
│   │   └── BdCategoria.cs      # Clases de acceso a datos
│   ├── ModelArticulo.cs         # Entidad Artículo
│   ├── ModelCategoria.cs        # Entidad Categoría
│   ├── ModelAutor.cs            # Entidad Autor
│   ├── ModelEtiqueta.cs         # Entidad Etiqueta
│   ├── ModelFuente.cs           # Entidad Fuente
│   ├── ModelMedio.cs            # Entidad Medio
│   └── ModelArticulo*.cs        # Tablas de unión (relaciones N:M)
├── Views/                       # Vistas XAML
├── Controllers/                 # Controladores
├── Services/                    # Servicios de negocio
├── Migrations/                  # Migraciones de EF Core
├── App.xaml                     # Configuración de aplicación
├── MainWindow.xaml              # Ventana principal
└── Enkarta.csproj              # Archivo de proyecto
```

## Modelo de Datos

El proyecto utiliza las siguientes entidades principales:

- **Articulo**: Artículos con título, resumen, contenido, palabras clave
- **Categoria**: Categorías para clasificar artículos
- **Autor**: Autores de artículos
- **Etiqueta**: Etiquetas/tags para artículos
- **Fuente**: Fuentes/referencias de artículos
- **Medio**: Archivos multimedia asociados a artículos

### Relaciones

- Un **Artículo** pertenece a una **Categoría** (N:1)
- Un **Artículo** puede tener múltiples **Autores** (N:M)
- Un **Artículo** puede tener múltiples **Etiquetas** (N:M)
- Un **Artículo** puede tener múltiples **Fuentes** (N:M)
- Un **Artículo** puede tener múltiples **Medios** (N:M)

## Entity Framework Core

### Comandos Útiles

#### Crear una nueva migración

Después de modificar los modelos:

```bash
dotnet ef migrations add NombreDeLaMigracion
```

#### Aplicar migraciones pendientes

```bash
dotnet ef database update
```

#### Revertir a una migración específica

```bash
dotnet ef database update NombreDeLaMigracion
```

#### Eliminar la última migración (sin aplicar)

```bash
dotnet ef migrations remove
```

#### Ver el SQL que generará una migración

```bash
dotnet ef migrations script
```

### Ejemplo de Uso del DbContext

```csharp
using Enkarta.Models.Conex;
using Microsoft.EntityFrameworkCore;

// Consultar artículos con sus categorías
using (var db = new DbConex())
{
    var articulos = db.Articulos
        .Where(a => a.Estado)
        .Include(a => a.Categoria)
        .ToList();
}

// Crear un nuevo artículo
using (var db = new DbConex())
{
    var articulo = new ModelArticulo
    {
        Titulo = "Mi primer artículo",
        Resumen = "Este es un resumen",
        Contenido = "Contenido del artículo...",
        CategoriaId = 1,
        FechaPublicacion = DateTime.Now
    };

    db.Articulos.Add(articulo);
    db.SaveChanges();
}
```

## Solución de Problemas

### Error: "No se pudo ejecutar porque no se encontró el comando dotnet-ef"

Instala la herramienta global de EF Core:

```bash
dotnet tool install --global dotnet-ef
```

### Error: "Unable to connect to any of the specified MySQL hosts"

Verifica que:
1. MySQL esté corriendo (en XAMPP, asegúrate de que el servicio MySQL esté activo)
2. La cadena de conexión en `DbConex.cs` sea correcta
3. El puerto sea el correcto (por defecto 3306)

### Error de compilación sobre Entity Framework

Restaura los paquetes NuGet:

```bash
dotnet restore
dotnet clean
dotnet build
```

### La base de datos no se crea

Asegúrate de:
1. Que MySQL esté corriendo
2. Que tengas permisos para crear bases de datos
3. Ejecuta `dotnet ef database update` nuevamente con la opción verbose:

```bash
dotnet ef database update --verbose
```

## Contribuir

1. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
2. Haz commit de tus cambios (`git commit -m 'Agregar nueva funcionalidad'`)
3. Push a la rama (`git push origin feature/nueva-funcionalidad`)
4. Abre un Pull Request

## Convenciones de Código

- Las clases de modelo usan el prefijo `Model` (ej: `ModelArticulo`)
- Las clases de acceso a datos usan el prefijo `Bd` (ej: `BdCategoria`)
- Las propiedades usan nombres en español que reflejan el dominio del negocio
- Todas las entidades principales tienen soft delete (`Estado` y `FechaDesactivado`)
- Todas las entidades tienen timestamps de auditoría (`FechaCreado`, `FechaActualizado`)

## Licencia

[Especificar licencia]

## Contacto

[Agregar información de contacto del equipo]
