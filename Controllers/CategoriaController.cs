using System;
using System.Collections.Generic;
using Enkarta.Models;
using Enkarta.Services;

namespace Enkarta.Controllers
{
    public class CategoriaController
    {
        private readonly CategoriaService _servicio;

        public CategoriaController()
        {
            _servicio = new CategoriaService();
        }

        // Listar todas las categorías
        public List<ModelCategoria> ListarCategorias()
        {
            return _servicio.ListarCategorias();
        }

        // Buscar categoría por ID
        public ModelCategoria? BuscarPorId(int id)
        {
            return _servicio.BuscarPorId(id);
        }

        // Crear nueva categoría
        public bool CrearCategoria(ModelCategoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                throw new ArgumentException("El nombre de la categoría es obligatorio");
            }

            if (categoria.Nombre.Length < 3)
            {
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres");
            }

            return _servicio.CrearCategoria(categoria);
        }

        // Actualizar categoría
        public bool ActualizarCategoria(ModelCategoria categoria)
        {
            if (categoria.Id <= 0)
            {
                throw new ArgumentException("ID de categoría inválido");
            }

            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                throw new ArgumentException("El nombre de la categoría es obligatorio");
            }

            if (categoria.Nombre.Length < 3)
            {
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres");
            }

            return _servicio.ActualizarCategoria(categoria);
        }

        // Eliminar categoría
        public bool EliminarCategoria(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID de categoría inválido");
            }

            return _servicio.EliminarCategoria(id);
        }

        // Buscar categorías por nombre
        public List<ModelCategoria> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return ListarCategorias();
            }

            return _servicio.BuscarPorNombre(nombre);
        }

        // Contar artículos
        public int ContarArticulosPorCategoria(int categoriaId)
        {
            return _servicio.ContarArticulosPorCategoria(categoriaId);
        }
    }
}