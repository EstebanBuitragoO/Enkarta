using System;
using System.Collections.Generic;

namespace Enkarta.Models
{
    public class ModelEtiqueta
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaDesactivado { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaActualizado { get; set; }

        // Propiedades de navegación
        public ICollection<ModelArticuloEtiqueta> ArticuloEtiquetas { get; set; } = new List<ModelArticuloEtiqueta>();
    }
}
