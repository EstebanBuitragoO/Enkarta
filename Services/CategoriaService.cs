using System;
using Enkarta.Models;
using Enkarta.Models.Conex;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enkarta.Services
{
    public class CategoriaService
    {
        private readonly DbConex _context;

        public CategoriaService()
        {
            _context = new DbConex();
        }

        // Listar categorías activas
        public List<ModelCategoria> ListarCategorias()
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
                Console.WriteLine($"Error al listar categorías: {ex.Message}");
                return new List<ModelCategoria>();
            }
        }

        // Buscar categoría por ID
        public ModelCategoria? BuscarPorId(int id)
        {
            try
            {
                return _context.Categorias.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar categoría: {ex.Message}");
                return null;
            }
        }

        // Crear nueva categoría
        public bool CrearCategoria(ModelCategoria categoria)
        {
            try
            {
                categoria.FechaCreado = DateTime.Now;
                categoria.Estado = true;

                _context.Categorias.Add(categoria);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear categoría: {ex.Message}");
                return false;
            }
        }

        // Actualizar categoría existente
        public bool ActualizarCategoria(ModelCategoria categoria)
        {
            try
            {
                var categoriaExistente = _context.Categorias.Find(categoria.Id);
                if (categoriaExistente == null) return false;

                categoriaExistente.Nombre = categoria.Nombre;
                categoriaExistente.Descripcion = categoria.Descripcion;
                categoriaExistente.FechaActualizado = DateTime.Now;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar categoría: {ex.Message}");
                return false;
            }
        }

        // Eliminar categoría 
        public bool EliminarCategoria(int id)
        {
            try
            {
                var categoria = _context.Categorias.Find(id);
                if (categoria == null) return false;

                categoria.Estado = false;
                categoria.FechaDesactivado = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar categoría: {ex.Message}");
                return false;
            }
        }

        // Buscar categorías por nombre
        public List<ModelCategoria> BuscarPorNombre(string nombre)
        {
            try
            {
                return _context.Categorias
                    .Where(c => c.Estado == true &&
                                c.Nombre != null &&
                                c.Nombre.Contains(nombre))
                    .OrderBy(c => c.Nombre)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar categorías: {ex.Message}");
                return new List<ModelCategoria>();
            }
        }

        // Contar artículos por categoría
        public int ContarArticulosPorCategoria(int categoriaId)
        {
            try
            {
                return _context.Articulos
                    .Count(a => a.CategoriaId == categoriaId && a.Estado == true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al contar artículos: {ex.Message}");
                return 0;
            }
        }
    }
}