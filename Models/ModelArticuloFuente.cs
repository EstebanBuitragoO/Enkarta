using System;

namespace Enkarta.Models
{
    public class ModelArticuloFuente
    {
        public int Id { get; set; }
        public int ArticuloId { get; set; }
        public int FuenteId { get; set; }

        // Propiedades de navegación
        public ModelArticulo? Articulo { get; set; }
        public ModelFuente? Fuente { get; set; }
    }
}
