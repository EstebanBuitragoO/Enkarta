using System;
using Enkarta.Models;
using Enkarta.Models.Conex;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Enkarta.Services
{
    public class ArticuloService
    {

        private readonly DbConex _context;

        public ArticuloService()
        {
            _context = new DbConex();
        }

        // Listar articulos
        public List<ModelArticulo> ListarArticulos()
        {
            try
            {
                return _context.Articulos
                  .Include(a => a.Categoria)
                  .Include(a => a.ArticuloAutores)
                      .ThenInclude(aa => aa.Autor)
                  .Where(a => a.Estado == true)
                  .OrderByDescending(a => a.FechaCreado)
                  .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error  al listar articulos: {ex.Message}");
                return new List<ModelArticulo>();
            }
        }

        // Buscar articulo por ID
        public ModelArticulo? BuscarPorId(int id)
        {
            try
            {
                return _context.Articulos
                    .Include(a => a.Categoria)
                    .Include(a => a.ArticuloAutores)
                        .ThenInclude(aa => aa.Autor)
                    .Include(a => a.ArticuloEtiquetas)
                        .ThenInclude(ae => ae.Etiqueta)
                    .Include(a => a.ArticuloFuentes)
                        .ThenInclude(af => af.Fuente)
                    .Include(a => a.ArticuloMedios)
                        .ThenInclude(am => am.Medio)
                    .FirstOrDefault(a => a.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar artículo: {ex.Message}");
                return null;
            }
        }

        // Crear artículo
        public bool CrearArticulo(ModelArticulo articulo, List<int> autoresIds, List<int> etiquetasIds, List<int> fuentesIds, List<int> mediosIds)
        {
            try
            {
                articulo.FechaCreado = DateTime.Now;
                articulo.Estado = true;

                _context.Articulos.Add(articulo);
                _context.SaveChanges();

                if (autoresIds != null && autoresIds.Any())
                {
                    foreach (var autorId in autoresIds)
                    {
                        _context.ArticuloAutores.Add(new ModelArticuloAutor
                        {
                            ArticuloId = articulo.Id,
                            AutorId = autorId
                        });
                    }
                }

                if (etiquetasIds != null && etiquetasIds.Any())
                {
                    foreach (var etiquetaId in etiquetasIds)
                    {
                        _context.ArticuloEtiquetas.Add(new ModelArticuloEtiqueta
                        {
                            ArticuloId = articulo.Id,
                            EtiquetaId = etiquetaId
                        });
                    }
                }

                if (fuentesIds != null && fuentesIds.Any())
                {
                    foreach (var fuenteId in fuentesIds)
                    {
                        _context.ArticuloFuentes.Add(new ModelArticuloFuente
                        {
                            ArticuloId = articulo.Id,
                            FuenteId = fuenteId
                        });
                    }
                }

                if (mediosIds != null && mediosIds.Any())
                {
                    foreach (var medioId in mediosIds)
                    {
                        _context.ArticuloMedios.Add(new ModelArticuloMedio
                        {
                            ArticuloId = articulo.Id,
                            MedioId = medioId
                        });
                    }
                }


                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear artículo: {ex.Message}");
                return false;
            }
        }

        // Actualizar artículo
        public bool ActualizarArticulo(ModelArticulo articulo, List<int> autoresIds, List<int> etiquetasIds, List<int> fuentesIds, List<int> mediosIds)
        {
            try
            {
                var articuloExistente = _context.Articulos.Find(articulo.Id);
                if (articuloExistente == null) return false;

                articuloExistente.Titulo = articulo.Titulo;
                articuloExistente.Resumen = articulo.Resumen;
                articuloExistente.Contenido = articulo.Contenido;
                articuloExistente.CategoriaId = articulo.CategoriaId;
                articuloExistente.FechaPublicacion = articulo.FechaPublicacion;
                articuloExistente.PalabrasClaves = articulo.PalabrasClaves;
                articuloExistente.FechaActualizado = DateTime.Now;

                var autoresActuales = _context.ArticuloAutores
                    .Where(aa => aa.ArticuloId == articulo.Id)
                    .ToList();
                _context.ArticuloAutores.RemoveRange(autoresActuales);

                if (autoresIds != null && autoresIds.Any())
                {
                    foreach (var autorId in autoresIds)
                    {
                        _context.ArticuloAutores.Add(new ModelArticuloAutor
                        {
                            ArticuloId = articulo.Id,
                            AutorId = autorId
                        });
                    }
                }

                var etiquetasActuales = _context.ArticuloEtiquetas
                       .Where(ae => ae.ArticuloId == articulo.Id)
                       .ToList();
                _context.ArticuloEtiquetas.RemoveRange(etiquetasActuales);

                if (etiquetasIds != null && etiquetasIds.Any())
                {
                    foreach (var etiquetaId in etiquetasIds)
                    {
                        _context.ArticuloEtiquetas.Add(new ModelArticuloEtiqueta
                        {
                            ArticuloId = articulo.Id,
                            EtiquetaId = etiquetaId
                        });
                    }
                }

                var fuentesActuales = _context.ArticuloFuentes
                .Where(af => af.ArticuloId == articulo.Id)
                .ToList();
                _context.ArticuloFuentes.RemoveRange(fuentesActuales);

                if (fuentesIds != null && fuentesIds.Any())
                {
                    foreach (var fuenteId in fuentesIds)
                    {
                        _context.ArticuloFuentes.Add(new ModelArticuloFuente
                        {
                            ArticuloId = articulo.Id,
                            FuenteId = fuenteId
                        });
                    }
                }

                var mediosActuales = _context.ArticuloMedios
                    .Where(am => am.ArticuloId == articulo.Id)
                    .ToList();
                _context.ArticuloMedios.RemoveRange(mediosActuales);

                if (mediosIds != null && mediosIds.Any())
                {
                    foreach (var medioId in mediosIds)
                    {
                        _context.ArticuloMedios.Add(new ModelArticuloMedio
                        {
                            ArticuloId = articulo.Id,
                            MedioId = medioId
                        });
                    }
                }


                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar artículo: {ex.Message}");
                return false;
            }
        }

        // Eliminar artículo
        public bool EliminarArticulo(int id)
        {
            try
            {
                var articulo = _context.Articulos.Find(id);
                if (articulo == null) return false;

                articulo.Estado = false;
                articulo.FechaDesactivado = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar artículo: {ex.Message}");
                return false;
            }
        }

        // Obtener las categorias activas
        public List<ModelCategoria> ObtenerCategorias()
        {
            try
            {
                return _context.Categorias
                    .Where(c => c.Estado == true)
                    .OrderBy(c => c.Nombre)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categorías: {ex.Message}");
                return new List<ModelCategoria>();
            }
        }

        // Obtener todos los autores activos
        public List<ModelAutor> ObtenerAutores()
        {
            try
            {
                return _context.Autores
                    .Where(a => a.Estado == true)
                    .OrderBy(a => a.Apellidos)
                    .ThenBy(a => a.Nombres)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener autores: {ex.Message}");
                return new List<ModelAutor>();
            }
        }

        // Obtener todas las etiquetas activas
        public List<ModelEtiqueta> ObtenerEtiquetas()
        {
            try
            {
                return _context.Etiquetas
                    .Where(e => e.Estado == true)
                    .OrderBy(e => e.Nombre)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener etiquetas: {ex.Message}");
                return new List<ModelEtiqueta>();
            }
        }

        // Búsqueda avanzada
        public List<ModelArticulo> BusquedaAvanzada(string? textoBusqueda, int? categoriaId, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                var query = _context.Articulos
                    .Include(a => a.Categoria)
                    .Include(a => a.ArticuloAutores)
                        .ThenInclude(aa => aa.Autor)
                    .Where(a => a.Estado == true)
                    .AsQueryable();

                // Buscar sólo por Título
                if (!string.IsNullOrWhiteSpace(textoBusqueda))
                {
                    var texto = textoBusqueda.Trim();
                    query = query.Where(a => a.Titulo != null && a.Titulo.Contains(texto));
                }

                // Filtrar por categoría
                if (categoriaId.HasValue && categoriaId.Value > 0)
                {
                    query = query.Where(a => a.CategoriaId == categoriaId.Value);
                }

                // Filtrar por rango de FechaPublicacion (solo artículos con FechaPublicacion definida)
                if (fechaDesde.HasValue)
                {
                    query = query.Where(a => a.FechaPublicacion.HasValue && a.FechaPublicacion.Value >= fechaDesde.Value);
                }

                if (fechaHasta.HasValue)
                {
                    query = query.Where(a => a.FechaPublicacion.HasValue && a.FechaPublicacion.Value <= fechaHasta.Value);
                }

                return query.OrderByDescending(a => a.FechaPublicacion).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en búsqueda avanzada: {ex.Message}");
                return new List<ModelArticulo>();
            }
        }
    }
}