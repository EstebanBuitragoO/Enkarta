using System;
using System.Collections.Generic;
using Enkarta.Models;
using Enkarta.Services;

namespace Enkarta.Controllers
{
    public class ArticuloController
    {
        private readonly ArticuloService _servicio;

        public ArticuloController()
        {
            _servicio = new ArticuloService();
        }

        public List<ModelArticulo> ListarArticulos()
        {
            return _servicio.ListarArticulos();
        }

        public ModelArticulo? BuscarPorId(int id)
        {
            return _servicio.BuscarPorId(id);
        }

        public bool CrearArticulo(ModelArticulo articulo, List<int> autoresIds, List<int> etiquetasIds, List<int> fuentesIds, List<int> mediosIds)
        {
            if (string.IsNullOrWhiteSpace(articulo.Titulo))
            {
                throw new ArgumentException("El título es obligatorio");
            }

            if (articulo.CategoriaId <= 0)
            {
                throw new ArgumentException("Debe seleccionar una categoría");
            }

            return _servicio.CrearArticulo(articulo, autoresIds, etiquetasIds, fuentesIds, mediosIds);
        }

        // Actualizar artículo
        public bool ActualizarArticulo(ModelArticulo articulo, List<int> autoresIds, List<int> etiquetasIds, List<int> fuentesIds, List<int> mediosIds)
        {
            if (articulo.Id <= 0)
            {
                throw new ArgumentException("ID de artículo inválido");
            }

            if (string.IsNullOrWhiteSpace(articulo.Titulo))
            {
                throw new ArgumentException("El título es obligatorio");
            }

            if (articulo.CategoriaId <= 0)
            {
                throw new ArgumentException("Debe seleccionar una categoría");
            }

            return _servicio.ActualizarArticulo(articulo, autoresIds, etiquetasIds, fuentesIds, mediosIds);
        }

        // Eliminar artículo 
        public bool EliminarArticulo(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID de artículo inválido");
            }

            return _servicio.EliminarArticulo(id);
        }

        // Obtener categorías para ComboBox
        public List<ModelCategoria> ObtenerCategorias()
        {
            return _servicio.ObtenerCategorias();
        }

        // Obtener autores para selección múltiple
        public List<ModelAutor> ObtenerAutores()
        {
            return _servicio.ObtenerAutores();
        }

        // Obtener etiquetas para selección múltiple
        public List<ModelEtiqueta> ObtenerEtiquetas()
        {
            return _servicio.ObtenerEtiquetas();
        }

        // Búsqueda avanzada de artículos
        public List<ModelArticulo> BusquedaAvanzada(string? textoBusqueda, int? categoriaId, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return _servicio.BusquedaAvanzada(textoBusqueda, categoriaId, fechaDesde, fechaHasta);
        }
    }
}