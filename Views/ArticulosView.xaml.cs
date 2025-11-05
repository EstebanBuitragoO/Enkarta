using Enkarta.Controllers;
using Enkarta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Enkarta.Views
{
    public partial class ArticulosView : UserControl
    {
        private readonly ArticuloController _controller;
        private ModelArticulo? _articuloSeleccionado;
        private bool _modoEdicion = false;

        public ArticulosView()
        {
            InitializeComponent();
            _controller = new ArticuloController();
            CargarDatos();
        }

        // Cargar todos los datos iniciales
        private void CargarDatos()
        {
            CargarArticulos();
            CargarCategorias();
            // No cargamos autores/etiquetas: ya no forman parte del formulario
        }

        // Cargar lista de artículos en el DataGrid
        private void CargarArticulos()
        {
            try
            {
                var articulos = _controller.ListarArticulos();
                dgArticulos.ItemsSource = articulos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar artículos: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Cargar categorías en el ComboBox
        private void CargarCategorias()
        {
            try
            {
                var categorias = _controller.ObtenerCategorias();
                cmbCategoria.ItemsSource = categorias;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Evento: Selección cambiada en el DataGrid
        private void DgArticulos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgArticulos.SelectedItem is ModelArticulo articulo)
            {
                _articuloSeleccionado = articulo;
                CargarArticuloEnFormulario(articulo);
                btnEliminar.IsEnabled = true;
                _modoEdicion = true;
                lblTituloFormulario.Text = "Editar Artículo";

                // Colapsar panel izquierdo y dar espacio al formulario (comportamiento tipo Categorías)
                try
                {
                    RootGrid.ColumnDefinitions[0].Width = new GridLength(0);
                    RootGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                }
                catch { /* no crítico */ }
            }
        }

        // Cargar datos del artículo en el formulario
        private void CargarArticuloEnFormulario(ModelArticulo articulo)
        {
            // Recargar el artículo completo con todas sus relaciones
            var articuloCompleto = _controller.BuscarPorId(articulo.Id);
            if (articuloCompleto == null) return;

            txtTitulo.Text = articuloCompleto.Titulo;
            txtResumen.Text = articuloCompleto.Resumen;
            txtContenido.Text = articuloCompleto.Contenido;
            cmbCategoria.SelectedValue = articuloCompleto.CategoriaId;
            dpFechaPublicacion.SelectedDate = articuloCompleto.FechaPublicacion;
            txtPalabrasClaves.Text = articuloCompleto.PalabrasClaves;
            chkEstado.IsChecked = articuloCompleto.Estado;

            // Nota: autores/etiquetas removidos del formulario, no se seleccionan aquí
        }

        // Guardar (crear/actualizar)
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(txtTitulo.Text))
                {
                    MessageBox.Show("El título es obligatorio", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtTitulo.Focus();
                    return;
                }

                if (cmbCategoria.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una categoría", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbCategoria.Focus();
                    return;
                }

                // Crear objeto Artículo
                var articulo = new ModelArticulo
                {
                    Titulo = txtTitulo.Text.Trim(),
                    Resumen = txtResumen.Text.Trim(),
                    Contenido = txtContenido.Text.Trim(),
                    CategoriaId = (int)cmbCategoria.SelectedValue,
                    FechaPublicacion = dpFechaPublicacion.SelectedDate,
                    PalabrasClaves = txtPalabrasClaves.Text.Trim(),
                    Estado = chkEstado.IsChecked ?? true
                };

                // Como el formulario ya no contiene autores/etiquetas, enviamos listas vacías
                var autoresIds = new List<int>();
                var etiquetasIds = new List<int>();
                var fuentesIds = new List<int>();
                var mediosIds = new List<int>();

                bool resultado;

                if (_modoEdicion && _articuloSeleccionado != null)
                {
                    // Actualizar artículo existente
                    articulo.Id = _articuloSeleccionado.Id;
                    resultado = _controller.ActualizarArticulo(articulo, autoresIds, etiquetasIds, fuentesIds, mediosIds);

                    if (resultado)
                    {
                        MessageBox.Show("Artículo actualizado correctamente", "Éxito",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    // Crear nuevo artículo
                    resultado = _controller.CrearArticulo(articulo, autoresIds, etiquetasIds, fuentesIds, mediosIds);

                    if (resultado)
                    {
                        MessageBox.Show("Artículo creado correctamente", "Éxito",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                if (resultado)
                {
                    // Restaurar layout y recargar lista
                    RestaurarLayout();
                    CargarArticulos();
                    LimpiarFormulario();
                    _modoEdicion = false;
                    _articuloSeleccionado = null;
                    lblTituloFormulario.Text = "Nuevo Artículo";
                    btnEliminar.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Error al guardar el artículo", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Cancelar: limpiar y restaurar layout
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            RestaurarLayout();
            _modoEdicion = false;
            _articuloSeleccionado = null;
            lblTituloFormulario.Text = "Nuevo Artículo";
            btnEliminar.IsEnabled = false;
            dgArticulos.SelectedItem = null;
        }

        // Eliminar artículo
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (_articuloSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un artículo para eliminar", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"¿Está seguro de eliminar el artículo '{_articuloSeleccionado.Titulo}'?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                try
                {
                    var eliminado = _controller.EliminarArticulo(_articuloSeleccionado.Id);

                    if (eliminado)
                    {
                        MessageBox.Show("Artículo eliminado correctamente", "Éxito",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        RestaurarLayout();
                        CargarArticulos();
                        LimpiarFormulario();
                        _modoEdicion = false;
                        _articuloSeleccionado = null;
                        lblTituloFormulario.Text = "Nuevo Artículo";
                        btnEliminar.IsEnabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el artículo", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Restaurar layout original (cuando se cierra el formulario)
        private void RestaurarLayout()
        {
            try
            {
                RootGrid.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                RootGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
            }
            catch { /* no crítico */ }
        }

        // Botón: Refrescar (mantengo método si se usa en otro lugar)
        private void BtnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            CargarArticulos();
            txtBusquedaRapida.Clear();
        }

        // Búsqueda rápida mientras se escribe
        private void TxtBusquedaRapida_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Implementación básica de búsqueda en el DataGrid actual
            var texto = txtBusquedaRapida.Text.ToLower();

            if (string.IsNullOrWhiteSpace(texto))
            {
                CargarArticulos();
                return;
            }

            var articulos = _controller.ListarArticulos()
                .Where(a => (a.Titulo?.ToLower().Contains(texto) ?? false) ||
                           (a.Resumen?.ToLower().Contains(texto) ?? false) ||
                           (a.Categoria?.Nombre?.ToLower().Contains(texto) ?? false))
                .ToList();

            dgArticulos.ItemsSource = articulos;
        }

        // Limpiar todos los campos del formulario
        private void LimpiarFormulario()
        {
            txtTitulo.Clear();
            txtResumen.Clear();
            txtContenido.Clear();
            cmbCategoria.SelectedIndex = -1;
            dpFechaPublicacion.SelectedDate = null;
            txtPalabrasClaves.Clear();
            chkEstado.IsChecked = true;
        }
    }
}