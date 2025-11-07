# SCRIPT PARA LLENAR ENKARTA CON DATOS RANDOM
# Base de datos: enkarta
# Conexión: server=localhost;user=root;password=

USE enkarta;

# Limpiar datos existentes (opcional - descomenta si quieres limpiar primero)
# DELETE FROM ArticuloMedios;
# DELETE FROM ArticuloFuentes;
# DELETE FROM ArticuloEtiquetas;
# DELETE FROM ArticuloAutores;
# DELETE FROM Articulos;
# DELETE FROM Medios;
# DELETE FROM Fuentes;
# DELETE FROM Etiquetas;
# DELETE FROM Autores;
# DELETE FROM Categorias;

# ============================================
# 1. INSERTAR CATEGORÍAS (5)
# ============================================
INSERT INTO Categorias (Nombre, Descripcion, Estado)
VALUES
('Tecnología', 'Artículos relacionados con tecnología, software e innovación', 1),
('Educación', 'Contenido educativo, tutoriales y recursos de aprendizaje', 1),
('Ciencia', 'Investigaciones científicas y descubrimientos', 1),
('Negocios', 'Temas de administración, emprendimiento y economía', 1),
('Cultura', 'Arte, literatura, historia y expresiones culturales', 1);

# ============================================
# 2. INSERTAR AUTORES (5)
# ============================================
INSERT INTO Autores (Nombres, Apellidos, Biografia, Estado)
VALUES
('Juan', 'García López', 'Investigador en tecnología y desarrollo de software con 10 años de experiencia', 1),
('María', 'Rodríguez Martínez', 'Profesora universitaria especializada en educación digital e innovación pedagógica', 1),
('Carlos', 'Pérez Silva', 'Científico apasionado por la física cuántica y astrofísica', 1),
('Ana', 'López Fernández', 'Consultora de negocios y experta en emprendimiento sostenible', 1),
('Roberto', 'Martínez González', 'Historiador y crítico de arte con publicaciones internacionales', 1);

# ============================================
# 3. INSERTAR ETIQUETAS (5)
# ============================================
INSERT INTO Etiquetas (Nombre, Estado)
VALUES
('Python', 1),
('Machine Learning', 1),
('Tutorial', 1),
('Investigación', 1),
('Sostenibilidad', 1);

# ============================================
# 4. INSERTAR FUENTES (5)
# ============================================
INSERT INTO Fuentes (Titulo, Tipo, Url, Notas, Estado)
VALUES
('Stack Overflow', 'Sitio Web', 'https://stackoverflow.com', 'Comunidad de programadores con soluciones código', 1),
('MIT OpenCourseWare', 'Plataforma Educativa', 'https://ocw.mit.edu', 'Cursos del Instituto Tecnológico de Massachusetts', 1),
('Nature Journal', 'Revista Científica', 'https://www.nature.com', 'Una de las revistas científicas más prestigiosas del mundo', 1),
('Harvard Business Review', 'Publicación Empresarial', 'https://hbr.org', 'Revista de estrategia y negocios', 1),
('Smithsonian Magazine', 'Publicación Cultural', 'https://www.smithsonianmag.com', 'Revista de historia, arte y cultura', 1);

# ============================================
# 5. INSERTAR MEDIOS (5)
# ============================================
INSERT INTO Medios (Tipo, Titulo, Ruta, Descripcion, Estado)
VALUES
('Imagen', 'Diagrama de Arquitectura Python', '/imagenes/python-architecture.jpg', 'Diagrama que muestra la arquitectura de una aplicación Python', 1),
('Imagen', 'Gráfico de ML Performance', '/imagenes/ml-performance-chart.png', 'Gráfico comparativo del rendimiento de modelos de machine learning', 1),
('Video', 'Tutorial Principios Científicos', '/videos/science-principles-intro.mp4', 'Video introducción a los principios fundamentales de la ciencia', 1),
('Documento PDF', 'Guía de Emprendimiento', '/documentos/entrepreneurship-guide.pdf', 'Manual completo para iniciar un negocio exitoso', 1),
('Imagen', 'Galería Museo Virtual', '/imagenes/museum-gallery.jpg', 'Colección de obras de arte del museo virtual', 1);

# ============================================
# 6. INSERTAR ARTÍCULOS (5)
# ============================================
INSERT INTO Articulos (Titulo, Resumen, Contenido, CategoriaId, FechaPublicacion, PalabrasClaves, Estado)
VALUES
('Python para Machine Learning: Guía Completa', 'Descubre cómo usar Python y librerías como TensorFlow para crear modelos de machine learning robustos', 'En este artículo exploraremos paso a paso cómo construir un modelo de machine learning desde cero usando Python. Cubriremos conceptos fundamentales, preparación de datos, entrenamiento y evaluación...', 1, '2024-11-01', 'python, machine learning, tensorflow, inteligencia artificial', 1),
('Transformación Digital en la Educación Moderna', 'Cómo la tecnología está revolucionando los métodos de enseñanza y aprendizaje en las aulas', 'La educación está viviendo una transformación sin precedentes gracias a la adopción de tecnologías digitales. Desde plataformas de aprendizaje online hasta realidad virtual, los docentes tienen nuevas herramientas...', 2, '2024-10-28', 'educación, tecnología, digital, aprendizaje online', 1),
('Nuevos Descubrimientos en Física Cuántica', 'Investigadores descubren nuevas propiedades de partículas subatómicas que desafían teorías existentes', 'Un equipo internacional de científicos ha publicado recientemente hallazgos revolucionarios sobre el comportamiento de electrones en condiciones extremas. Estos descubrimientos abren nuevas posibilidades...', 3, '2024-10-25', 'física cuántica, investigación, ciencia, partículas', 1),
('Emprendimiento Sostenible: El Futuro de los Negocios', 'Cómo crear empresas rentables mientras se protege el medio ambiente', 'El emprendimiento sostenible no es solo una tendencia, es una necesidad. Las nuevas generaciones buscan empresas que generen impacto positivo. Te mostramos cómo combinar rentabilidad con responsabilidad...', 4, '2024-10-20', 'emprendimiento, sostenibilidad, negocios, empresa', 1),
('La Influencia del Arte Renacentista en el Mundo Moderno', 'Análisis de cómo los maestros renacentistas siguen inspirando a artistas contemporáneos', 'El Renacimiento marcó un hito en la historia del arte y la cultura. La obra de maestros como Leonardo da Vinci, Miguel Ángel y Rafael continúa influyendo en el arte moderno. En este artículo exploramos...', 5, '2024-10-15', 'arte, renacimiento, cultura, historia', 1);

# ============================================
# 7. ASOCIAR AUTORES A ARTÍCULOS (ArticuloAutores)
# ============================================
INSERT INTO ArticuloAutores (ArticuloId, AutorId)
VALUES
(1, 1),
(1, 2),
(2, 2),
(2, 4),
(3, 3),
(4, 4),
(4, 1),
(5, 5);

# ============================================
# 8. ASOCIAR ETIQUETAS A ARTÍCULOS (ArticuloEtiquetas)
# ============================================
INSERT INTO ArticuloEtiquetas (ArticuloId, EtiquetaId)
VALUES
(1, 1),
(1, 2),
(1, 3),
(2, 3),
(3, 4),
(4, 5);

# ============================================
# 9. ASOCIAR FUENTES A ARTÍCULOS (ArticuloFuentes)
# ============================================
INSERT INTO ArticuloFuentes (ArticuloId, FuenteId)
VALUES
(1, 1),
(1, 2),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

# ============================================
# 10. ASOCIAR MEDIOS A ARTÍCULOS (ArticuloMedios)
# ============================================
INSERT INTO ArticuloMedios (ArticuloId, MedioId)
VALUES
(1, 1),
(1, 2),
(2, 3),
(4, 4),
(5, 5);

# ============================================
# VERIFICACIÓN: Ver datos insertados
# ============================================
# SELECT COUNT(*) as TotalCategorias FROM Categorias;
# SELECT COUNT(*) as TotalAutores FROM Autores;
# SELECT COUNT(*) as TotalEtiquetas FROM Etiquetas;
# SELECT COUNT(*) as TotalFuentes FROM Fuentes;
# SELECT COUNT(*) as TotalMedios FROM Medios;
# SELECT COUNT(*) as TotalArticulos FROM Articulos;
# SELECT COUNT(*) as TotalArticuloAutores FROM ArticuloAutores;
# SELECT COUNT(*) as TotalArticuloEtiquetas FROM ArticuloEtiquetas;
# SELECT COUNT(*) as TotalArticuloFuentes FROM ArticuloFuentes;
# SELECT COUNT(*) as TotalArticuloMedios FROM ArticuloMedios;
