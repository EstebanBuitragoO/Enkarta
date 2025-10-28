using System;
using System.Collections.Generic;

namespace Enkarta.Models
{
    public class ModelMedio
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }
        public string? Titulo { get; set; }
        public string? Ruta { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaDesactivado { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaActualizado { get; set; }

        // Propiedades de navegación
        public ICollection<ModelArticuloMedio> ArticuloMedios { get; set; } = new List<ModelArticuloMedio>();
    }
}
