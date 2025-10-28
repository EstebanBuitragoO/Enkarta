using System;
using System.Collections.Generic;

namespace Enkarta.Models
{
    public class ModelFuente
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Tipo { get; set; }
        public string? Url { get; set; }
        public string? Notas { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaDesactivado { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaActualizado { get; set; }

        // Propiedades de navegación
        public ICollection<ModelArticuloFuente> ArticuloFuentes { get; set; } = new List<ModelArticuloFuente>();
    }
}
