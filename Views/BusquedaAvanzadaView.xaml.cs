using Enkarta.Controllers;
using Enkarta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Enkarta.Views
{
    public partial class BusquedaAvanzadaView : UserControl
    {
        private readonly ArticuloController _controller;
        private List<ModelArticulo> _resultados;
        private const string PlaceholderTexto = "Texto a buscar";

        public BusquedaAvanzadaView()
        {
            InitializeComponent();
            _controller = new ArticuloController();
            _resultados = new List<ModelArticulo>();
            CargarFiltros();

            // Placeholder simple para txtBusqueda
            txtBusqueda.Text = PlaceholderTexto;
            txtBusqueda.Foreground = TryFindResource("MutedTextBrush") as Brush ?? Brushes.Gray;
        }

        // Cargar datos para los filtros (solo categorías)
        private void CargarFiltros()
        {
            try
            {
                var categorias = _controller.ObtenerCategorias();
                categorias.Insert(0, new ModelCategoria { Id = 0, Nombre = "-- Todas --" });
                cmbCategoria.ItemsSource = categorias;
                cmbCategoria.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar filtros: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtBusqueda_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBusqueda.Text == PlaceholderTexto)
            {
                txtBusqueda.Text = string.Empty;
                txtBusqueda.Foreground = TryFindResource("TextBrush") as Brush ?? Brushes.Black;
            }
        }

        private void TxtBusqueda_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBusqueda.Text))
            {
                txtBusqueda.Text = PlaceholderTexto;
                txtBusqueda.Foreground = TryFindResource("MutedTextBrush") as Brush ?? Brushes.Gray;
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            RealizarBusqueda();
        }

        private void RealizarBusqueda()
        {
            try
            {
                string? textoBusqueda = (txtBusqueda.Text == PlaceholderTexto || string.IsNullOrWhiteSpace(txtBusqueda.Text))
                    ? null
                    : txtBusqueda.Text.Trim();

                int? categoriaId = cmbCategoria.SelectedValue != null && (int)cmbCategoria.SelectedValue > 0
                    ? (int)cmbCategoria.SelectedValue
                    : null;

                // Usamos una sola fecha (si existe)
                DateTime? fechaSeleccionada = dpFecha.SelectedDate;
                DateTime? fechaDesde = fechaSeleccionada;
                DateTime? fechaHasta = fechaSeleccionada;

                // Validar coherencia de fecha (aunque solo una)
                if (fechaDesde.HasValue && fechaHasta.HasValue && fechaDesde > fechaHasta)
                {
                    MessageBox.Show("La fecha seleccionada es inválida", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var articulos = _controller.BusquedaAvanzada(
                    textoBusqueda,
                    categoriaId,
                    fechaDesde,
                    fechaHasta
                );

                _resultados = articulos ?? new List<ModelArticulo>();

                dgResultados.ItemsSource = _resultados;
                lblResultados.Text = $"{_resultados.Count} artículo{(_resultados.Count != 1 ? "s" : "")} encontrado{(_resultados.Count != 1 ? "s" : "")}";

                if (_resultados.Count == 0)
                {
                    MessageBox.Show("No se encontraron artículos con los criterios especificados",
                        "Búsqueda", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al realizar la búsqueda: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Limpiar filtros
        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtBusqueda.Text = PlaceholderTexto;
            txtBusqueda.Foreground = TryFindResource("MutedTextBrush") as Brush ?? Brushes.Gray;
            cmbCategoria.SelectedIndex = 0;
            dpFecha.SelectedDate = null;
            dgResultados.ItemsSource = null;
            _resultados.Clear();
            lblResultados.Text = "0 artículos encontrados";
        }

        // Abrir reseña y opción de ir a gestión de fuentes
        private void BtnAbrir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null && int.TryParse(btn.Tag.ToString(), out int id))
            {
                var articulo = _controller.BuscarPorId(id);
                if (articulo == null)
                {
                    MessageBox.Show("No se encontró el artículo seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var reseña = new ResenaArticuloWindow(articulo) { Owner = Window.GetWindow(this) };
                var result = reseña.ShowDialog();

                // Si el usuario en la reseña pidió ir a Fuentes, navegamos
                if (result == true)
                {
                    // Buscar MainWindow y disparar el click del botón BtnFuentes
                    var main = System.Windows.Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w.GetType().Name == "MainWindow");

                    if (main != null)
                    {
                        var btnFuentes = main.FindName("BtnFuentes") as Button;
                        if (btnFuentes != null)
                        {
                            btnFuentes.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        }
                        else
                        {
                            MessageBox.Show("No se pudo navegar a Fuentes (control no encontrado).", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void DgResultados_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgResultados.SelectedItem is ModelArticulo articulo)
            {
                MostrarDetalleArticulo(articulo);
            }
        }

        private void MostrarDetalleArticulo(ModelArticulo articulo)
        {
            var autoresNombres = articulo.ArticuloAutores != null
                ? string.Join(", ", articulo.ArticuloAutores.Select(aa => $"{aa.Autor?.Apellidos} {aa.Autor?.Nombres}".Trim()).Where(s => !string.IsNullOrWhiteSpace(s)))
                : string.Empty;

            var etiquetasNombres = articulo.ArticuloEtiquetas != null
                ? string.Join(", ", articulo.ArticuloEtiquetas.Select(ae => ae.Etiqueta?.Nombre).Where(n => !string.IsNullOrWhiteSpace(n)))
                : string.Empty;

            var detalle = $"═══════════════════════════════════\n" +
                          $"DETALLE DEL ARTÍCULO\n" +
                          $"═══════════════════════════════════\n\n" +
                          $"ID: {articulo.Id}\n" +
                          $"Título: {articulo.Titulo}\n" +
                          $"Categoría: {articulo.Categoria?.Nombre ?? "N/A"}\n" +
                          $"Fecha Publicación: {articulo.FechaPublicacion?.ToString("dd/MM/yyyy") ?? "N/A"}\n\n" +
                          $"Resumen:\n{(string.IsNullOrEmpty(articulo.Resumen) ? "  N/A" : "  " + articulo.Resumen)}\n\n" +
                          $"═══════════════════════════════════";

            MessageBox.Show(detalle, "Detalle del Artículo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}