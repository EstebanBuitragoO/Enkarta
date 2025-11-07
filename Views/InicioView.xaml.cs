using Enkarta.Controllers;
using Enkarta.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Enkarta.Views
{
    public partial class InicioView : UserControl
    {
        private readonly ArticuloController _articuloController;
        private readonly CategoriaController _categoriaController;

        // Evento para notificar a MainWindow que se debe navegar
        public event EventHandler<NavigationEventArgs>? NavigationRequested;

        public InicioView()
        {
            InitializeComponent();
            _articuloController = new ArticuloController();
            _categoriaController = new CategoriaController();

            CargarEstadisticas();
            CargarUltimosArticulos();
        }

        /// <summary>
        /// Carga las estadísticas rápidas del sistema
        /// </summary>
        private void CargarEstadisticas()
        {
            try
            {
                // Contar artículos
                var articulos = _articuloController.ListarArticulos();
                lblTotalArticulos.Text = articulos.Count.ToString();

                // Contar categorías
                var categorias = _categoriaController.ListarCategorias();
                lblTotalCategorias.Text = categorias.Count.ToString();

                // Contar autores (contaremos los únicos autores asociados a artículos)
                int totalAutores = 0;
                foreach (var articulo in articulos)
                {
                    var articuloCompleto = _articuloController.BuscarPorId(articulo.Id);
                    if (articuloCompleto?.ArticuloAutores != null)
                    {
                        totalAutores += articuloCompleto.ArticuloAutores.Count;
                    }
                }
                lblTotalAutores.Text = totalAutores.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar estadísticas: {ex.Message}");
            }
        }

        /// <summary>
        /// Carga los últimos 3 artículos para mostrar en la tabla
        /// </summary>
        private void CargarUltimosArticulos()
        {
            try
            {
                var articulos = _articuloController.ListarArticulos();

                // Tomar los últimos 3 artículos (ordenados por ID descendente)
                var ultimosArticulos = articulos
                    .OrderByDescending(a => a.Id)
                    .Take(3)
                    .ToList();

                dgUltimosArticulos.ItemsSource = ultimosArticulos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar últimos artículos: {ex.Message}");
            }
        }

        /// <summary>
        /// Evento: Nuevo Artículo desde Inicio
        /// </summary>
        private void BtnNuevoArticuloDesdeInicio_Click(object sender, RoutedEventArgs e)
        {
            // Disparar el evento de navegación
            NavigationRequested?.Invoke(this, new NavigationEventArgs { PageName = "Articulos", Action = "NuevoArticulo" });
        }

        /// <summary>
        /// Evento: Nueva Categoría desde Inicio
        /// </summary>
        private void BtnNuevaCategoriasDesdeInicio_Click(object sender, RoutedEventArgs e)
        {
            // Disparar el evento de navegación
            NavigationRequested?.Invoke(this, new NavigationEventArgs { PageName = "Categorias", Action = "NuevaCategoria" });
        }

        /// <summary>
        /// Evento: Selección de artículo en la tabla de últimos artículos
        /// </summary>
        private void DgUltimosArticulos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUltimosArticulos.SelectedItem is ModelArticulo articulo)
            {
                // Disparar el evento de navegación a Artículos con acción "VerArticulo"
                NavigationRequested?.Invoke(this, new NavigationEventArgs
                {
                    PageName = "Articulos",
                    Action = "VerArticulo",
                    ArticuloId = articulo.Id
                });
            }
        }
    }

    /// <summary>
    /// Clase para pasar información de navegación
    /// </summary>
    public class NavigationEventArgs : EventArgs
    {
        public string? PageName { get; set; }
        public string? Action { get; set; }
        public int? ArticuloId { get; set; }
    }
}
