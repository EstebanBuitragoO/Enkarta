using Enkarta.Controllers;
using System;
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
            CargarFechaActualizacion();
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
        /// Carga la fecha de última actualización del sistema
        /// </summary>
        private void CargarFechaActualizacion()
        {
            try
            {
                // Aquí puedes obtener la fecha del último artículo modificado
                var articulos = _articuloController.ListarArticulos();
                if (articulos.Count > 0)
                {
                    var ultimaActualizacion = articulos[0].FechaActualizado;
                    if (ultimaActualizacion != null)
                    {
                        lblLastUpdate.Text = ultimaActualizacion.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        lblLastUpdate.Text = "Sin actualización registrada";
                    }
                }
                else
                {
                    lblLastUpdate.Text = "Sistema sin datos";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar fecha: {ex.Message}");
                lblLastUpdate.Text = "Error al cargar";
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
    }

    /// <summary>
    /// Clase para pasar información de navegación
    /// </summary>
    public class NavigationEventArgs : EventArgs
    {
        public string? PageName { get; set; }
        public string? Action { get; set; }
    }
}
