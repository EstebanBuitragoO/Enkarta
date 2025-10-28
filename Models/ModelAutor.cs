using System;
using System.Collections.Generic;

namespace Enkarta.Models
{
    public class ModelAutor
    {
        public int Id { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Biografia { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaDesactivado { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaActualizado { get; set; }

        // Propiedades de navegación
        public ICollection<ModelArticuloAutor> ArticuloAutores { get; set; } = new List<ModelArticuloAutor>();
    }
}
