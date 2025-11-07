# ============================================
# CREAR BASE DE DATOS ENKARTA DESDE CERO
# ============================================

# Crear la base de datos
CREATE DATABASE IF NOT EXISTS enkarta;

# Usar la base de datos
USE enkarta;

# ============================================
# 1. CREAR TABLA CATEGORIAS
# ============================================
CREATE TABLE Categorias (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nombre VARCHAR(255) NOT NULL,
    Descripcion LONGTEXT,
    Estado BOOLEAN DEFAULT 1,
    FechaDesactivado DATETIME NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_estado (Estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# 2. CREAR TABLA AUTORES
# ============================================
CREATE TABLE Autores (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nombres VARCHAR(255) NOT NULL,
    Apellidos VARCHAR(255) NOT NULL,
    Biografia LONGTEXT,
    Estado BOOLEAN DEFAULT 1,
    FechaDesactivado DATETIME NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_estado (Estado),
    INDEX idx_nombre (Nombres, Apellidos)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# 3. CREAR TABLA ETIQUETAS
# ============================================
CREATE TABLE Etiquetas (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nombre VARCHAR(255) NOT NULL UNIQUE,
    Estado BOOLEAN DEFAULT 1,
    FechaDesactivado DATETIME NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_estado (Estado),
    INDEX idx_nombre (Nombre)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# 4. CREAR TABLA FUENTES
# ============================================
CREATE TABLE Fuentes (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Titulo VARCHAR(255) NOT NULL,
    Tipo VARCHAR(100),
    Url VARCHAR(500),
    Notas LONGTEXT,
    Estado BOOLEAN DEFAULT 1,
    FechaDesactivado DATETIME NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_estado (Estado),
    INDEX idx_tipo (Tipo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# 5. CREAR TABLA MEDIOS
# ============================================
CREATE TABLE Medios (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Tipo VARCHAR(100),
    Titulo VARCHAR(255) NOT NULL,
    Ruta VARCHAR(500),
    Descripcion LONGTEXT,
    Estado BOOLEAN DEFAULT 1,
    FechaDesactivado DATETIME NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_estado (Estado),
    INDEX idx_tipo (Tipo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# 6. CREAR TABLA ARTICULOS
# ============================================
CREATE TABLE Articulos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Titulo VARCHAR(500) NOT NULL,
    Resumen LONGTEXT,
    Contenido LONGTEXT,
    CategoriaId INT NOT NULL,
    FechaPublicacion DATETIME,
    PalabrasClaves TEXT,
    Estado BOOLEAN DEFAULT 1,
    FechaDesactivado DATETIME NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_estado (Estado),
    INDEX idx_categoria (CategoriaId),
    INDEX idx_fecha_publicacion (FechaPublicacion),
    CONSTRAINT fk_articulo_categoria FOREIGN KEY (CategoriaId)
        REFERENCES Categorias(Id) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# 7. CREAR TABLA ARTICULO_AUTORES (N:M)
# ============================================
CREATE TABLE ArticuloAutores (
    ArticuloId INT NOT NULL,
    AutorId INT NOT NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (ArticuloId, AutorId),
    CONSTRAINT fk_articulo_autor_articulo FOREIGN KEY (ArticuloId)
        REFERENCES Articulos(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_articulo_autor_autor FOREIGN KEY (AutorId)
        REFERENCES Autores(Id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# 8. CREAR TABLA ARTICULO_ETIQUETAS (N:M)
# ============================================
CREATE TABLE ArticuloEtiquetas (
    ArticuloId INT NOT NULL,
    EtiquetaId INT NOT NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (ArticuloId, EtiquetaId),
    CONSTRAINT fk_articulo_etiqueta_articulo FOREIGN KEY (ArticuloId)
        REFERENCES Articulos(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_articulo_etiqueta_etiqueta FOREIGN KEY (EtiquetaId)
        REFERENCES Etiquetas(Id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# 9. CREAR TABLA ARTICULO_FUENTES (N:M)
# ============================================
CREATE TABLE ArticuloFuentes (
    ArticuloId INT NOT NULL,
    FuenteId INT NOT NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (ArticuloId, FuenteId),
    CONSTRAINT fk_articulo_fuente_articulo FOREIGN KEY (ArticuloId)
        REFERENCES Articulos(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_articulo_fuente_fuente FOREIGN KEY (FuenteId)
        REFERENCES Fuentes(Id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# 10. CREAR TABLA ARTICULO_MEDIOS (N:M)
# ============================================
CREATE TABLE ArticuloMedios (
    ArticuloId INT NOT NULL,
    MedioId INT NOT NULL,
    FechaCreado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (ArticuloId, MedioId),
    CONSTRAINT fk_articulo_medio_articulo FOREIGN KEY (ArticuloId)
        REFERENCES Articulos(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT fk_articulo_medio_medio FOREIGN KEY (MedioId)
        REFERENCES Medios(Id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

# ============================================
# CONFIRMACIÓN
# ============================================
# Base de datos creada exitosamente
# Ahora puedes ejecutar el script DatosSQL_Seed.sql para poblar los datos
