using System;

namespace Enkarta.Models
{
    public class ModelArticuloEtiqueta
    {
        public int Id { get; set; }
        public int ArticuloId { get; set; }
        public int EtiquetaId { get; set; }

        // Propiedades de navegación
        public ModelArticulo? Articulo { get; set; }
        public ModelEtiqueta? Etiqueta { get; set; }
    }
}
