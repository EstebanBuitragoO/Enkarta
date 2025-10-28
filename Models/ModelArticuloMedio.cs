using System;

namespace Enkarta.Models
{
    public class ModelArticuloMedio
    {
        public int Id { get; set; }
        public int ArticuloId { get; set; }
        public int MedioId { get; set; }
        public int? Orden { get; set; }

        // Propiedades de navegación
        public ModelArticulo? Articulo { get; set; }
        public ModelMedio? Medio { get; set; }
    }
}
