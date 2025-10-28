using System;

namespace Enkarta.Models
{
    public class ModelArticuloAutor
    {
        public int Id { get; set; }
        public int ArticuloId { get; set; }
        public int AutorId { get; set; }

        // Propiedades de navegación
        public ModelArticulo? Articulo { get; set; }
        public ModelAutor? Autor { get; set; }
    }
}
