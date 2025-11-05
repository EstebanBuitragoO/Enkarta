using System;
using System.Collections.Generic;

namespace Enkarta.Models
{
    public class ModelArticulo
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Resumen { get; set; }
        public string? Contenido { get; set; }
        public int CategoriaId { get; set; } // Clave foránea que indentifica a que catagoria pertenece mi artículo
        public DateTime? FechaPublicacion { get; set; }
        public string? PalabrasClaves { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaDesactivado { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaActualizado { get; set; }

        // Propiedades de navegación
        public ModelCategoria? Categoria { get; set; }// un artículo pertenece a una unica categoria
        public ICollection<ModelArticuloAutor> ArticuloAutores { get; set; } = new List<ModelArticuloAutor>();
        public ICollection<ModelArticuloEtiqueta> ArticuloEtiquetas { get; set; } = new List<ModelArticuloEtiqueta>();
        public ICollection<ModelArticuloFuente> ArticuloFuentes { get; set; } = new List<ModelArticuloFuente>();
        public ICollection<ModelArticuloMedio> ArticuloMedios { get; set; } = new List<ModelArticuloMedio>();
    }
}
