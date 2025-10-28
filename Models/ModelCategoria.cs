using System;
using System.Collections.Generic;

namespace Enkarta.Models
{
    public class ModelCategoria
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaDesactivado { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaActualizado { get; set; }

        // Propiedades de navegación
        public ICollection<ModelArticulo> Articulos { get; set; } = new List<ModelArticulo>();
    }
}
