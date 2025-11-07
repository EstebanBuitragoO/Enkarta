using Enkarta.Controllers;
using Enkarta.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                Debug.WriteLine("Hola, esto es debug"+articulos);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Hola, esto es debug" + ex.Message);
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

        // Evento: Clic en la DataGrid (para abrir vista de lectura al hacer clic en la fila)
        private void DgArticulos_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Obtener la fila donde se hizo clic
            var row = FindParentOfType<DataGridRow>(e.OriginalSource as DependencyObject);

            if (row != null && row.Item is ModelArticulo articulo)
            {
                // Si se hizo clic en un botón, no hacer nada aquí (el botón manejará su propio evento)
                var button = FindParentOfType<Button>(e.OriginalSource as DependencyObject);
                if (button != null)
                {
                    return;
                }

                // Si se hizo clic en la fila pero NO en un botón, mostrar la vista de lectura
                _articuloSeleccionado = articulo;
                CargarArticuloEnVista(articulo);

                // Ocultar panel de búsqueda
                SearchPanel.Visibility = Visibility.Collapsed;

                // Mostrar vista de lectura, ocultar tabla
                TablaPanel.Visibility = Visibility.Collapsed;
                ViewPanel.Visibility = Visibility.Visible;
            }
        }

        // Cargar artículo en vista de lectura
        private void CargarArticuloEnVista(ModelArticulo articulo)
        {
            var articuloCompleto = _controller.BuscarPorId(articulo.Id);
            if (articuloCompleto == null) return;

            lblViewTitulo.Text = articuloCompleto.Titulo;
            lblViewResumen.Text = articuloCompleto.Resumen ?? "Sin resumen";
            lblViewContenido.Text = articuloCompleto.Contenido ?? "Sin contenido";
            lblViewCategoria.Text = articuloCompleto.Categoria?.Nombre ?? "Sin categoría";
            lblViewFecha.Text = articuloCompleto.FechaPublicacion?.ToString("dd/MM/yyyy") ?? "Sin fecha";
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
                    // Recargar lista y volver a la tabla
                    CargarArticulos();
                    LimpiarFormulario();
                    _modoEdicion = false;
                    _articuloSeleccionado = null;
                    lblTituloFormulario.Text = "Nuevo Artículo";
                    btnEliminar.IsEnabled = false;

                    // Mostrar panel de búsqueda
                    SearchPanel.Visibility = Visibility.Visible;

                    // Mostrar tabla, ocultar formulario
                    TablaPanel.Visibility = Visibility.Visible;
                    FormPanel.Visibility = Visibility.Collapsed;
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

        // Botón Nuevo Artículo: mostrar formulario
        public void BtnNuevoArticulo_Click(object sender, RoutedEventArgs e)
        {
            _modoEdicion = false;
            _articuloSeleccionado = null;
            lblTituloFormulario.Text = "Nuevo Artículo";
            btnEliminar.IsEnabled = false;
            btnEliminar.Visibility = Visibility.Collapsed;
            LimpiarFormulario();
            dgArticulos.SelectedItem = null;

            // Ocultar panel de búsqueda
            SearchPanel.Visibility = Visibility.Collapsed;

            // Mostrar formulario, ocultar tabla
            TablaPanel.Visibility = Visibility.Collapsed;
            FormPanel.Visibility = Visibility.Visible;
        }

        // Cancelar: limpiar y restaurar layout
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            _modoEdicion = false;
            _articuloSeleccionado = null;
            lblTituloFormulario.Text = "Nuevo Artículo";
            btnEliminar.IsEnabled = false;
            dgArticulos.SelectedItem = null;

            // Mostrar panel de búsqueda
            SearchPanel.Visibility = Visibility.Visible;

            // Mostrar tabla, ocultar formulario
            TablaPanel.Visibility = Visibility.Visible;
            FormPanel.Visibility = Visibility.Collapsed;
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
                        CargarArticulos();
                        LimpiarFormulario();
                        _modoEdicion = false;
                        _articuloSeleccionado = null;
                        lblTituloFormulario.Text = "Nuevo Artículo";
                        btnEliminar.IsEnabled = false;

                        // Mostrar panel de búsqueda
                        SearchPanel.Visibility = Visibility.Visible;

                        // Mostrar tabla, ocultar formulario
                        TablaPanel.Visibility = Visibility.Visible;
                        FormPanel.Visibility = Visibility.Collapsed;
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

        // Botón Editar en la fila de la tabla
        private void BtnEditarFila_Click(object sender, RoutedEventArgs e)
        {
            // Marcar como manejado para evitar propagación
            e.Handled = true;

            if (sender is Button btn && btn.Tag is ModelArticulo articulo)
            {
                _articuloSeleccionado = articulo;
                _modoEdicion = true;
                CargarArticuloEnFormulario(articulo);
                btnEliminar.IsEnabled = true;
                btnEliminar.Visibility = Visibility.Visible;
                lblTituloFormulario.Text = "Editar Artículo";

                // Ocultar panel de búsqueda
                SearchPanel.Visibility = Visibility.Collapsed;

                // Mostrar formulario, ocultar tabla y vista de lectura
                TablaPanel.Visibility = Visibility.Collapsed;
                ViewPanel.Visibility = Visibility.Collapsed;
                FormPanel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Método auxiliar para encontrar un elemento padre de un tipo específico
        /// </summary>
        private T FindParentOfType<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;

            return FindParentOfType<T>(parentObject);
        }

        // Botón Editar desde la vista de lectura
        private void BtnEditarDesdeVista_Click(object sender, RoutedEventArgs e)
        {
            if (_articuloSeleccionado != null)
            {
                _modoEdicion = true;
                CargarArticuloEnFormulario(_articuloSeleccionado);
                btnEliminar.IsEnabled = true;
                btnEliminar.Visibility = Visibility.Visible;
                lblTituloFormulario.Text = "Editar Artículo";

                // Mostrar formulario, ocultar vista de lectura
                ViewPanel.Visibility = Visibility.Collapsed;
                FormPanel.Visibility = Visibility.Visible;
            }
        }

        // Botón Cerrar desde la vista de lectura
        private void BtnCerrarVista_Click(object sender, RoutedEventArgs e)
        {
            // Mostrar panel de búsqueda
            SearchPanel.Visibility = Visibility.Visible;

            // Mostrar tabla, ocultar vista de lectura
            TablaPanel.Visibility = Visibility.Visible;
            ViewPanel.Visibility = Visibility.Collapsed;
            dgArticulos.SelectedItem = null;
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

        /// <summary>
        /// Muestra la vista de lectura de un artículo específico (por ID)
        /// </summary>
        public void MostrarVistaArticulo(int articuloId)
        {
            try
            {
                var articulo = _controller.BuscarPorId(articuloId);
                if (articulo != null)
                {
                    _articuloSeleccionado = articulo;
                    CargarArticuloEnVista(articulo);

                    // Ocultar panel de búsqueda
                    SearchPanel.Visibility = Visibility.Collapsed;

                    // Mostrar vista de lectura, ocultar tabla
                    TablaPanel.Visibility = Visibility.Collapsed;
                    ViewPanel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el artículo: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
