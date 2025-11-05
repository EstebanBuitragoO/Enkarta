using Enkarta.Controllers;
using Enkarta.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Enkarta.Views
{
    public partial class CategoriasView : UserControl
    {
        private readonly CategoriaController _controller;
        private ModelCategoria? _categoriaSeleccionada;
        private bool _modoEdicion = false;

        public CategoriasView()
        {
            InitializeComponent();
            _controller = new CategoriaController();
            CargarCategorias();
        }

        // Cargar lista de categorías
        private void CargarCategorias()
        {
            try
            {
                var categorias = _controller.ListarCategorias();
                dgCategorias.ItemsSource = categorias;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Alternar entre vistas
        private void MostrarTabla()
        {
            VistaTabla.Visibility = Visibility.Visible;
            VistaFormulario.Visibility = Visibility.Collapsed;
        }

        private void MostrarFormulario()
        {
            VistaTabla.Visibility = Visibility.Collapsed;
            VistaFormulario.Visibility = Visibility.Visible;
        }

        private void BtnNuevaCategoria_Click(object sender, RoutedEventArgs e)
        {
            _modoEdicion = false;
            _categoriaSeleccionada = null;
            lblTituloFormulario.Text = "Nueva Categoría";
            LimpiarFormulario();
            MostrarFormulario();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ModelCategoria categoria)
            {
                _categoriaSeleccionada = categoria;
                _modoEdicion = true;
                lblTituloFormulario.Text = "Editar Categoría";
                CargarCategoriaEnFormulario(categoria);
                MostrarFormulario();
            }
        }

        // Cargar datos de categoría en el formulario
        private void CargarCategoriaEnFormulario(ModelCategoria categoria)
        {
            txtNombre.Text = categoria.Nombre;
            txtDescripcion.Text = categoria.Descripcion;
        }

        // Botón: Guardar
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar campo obligatorio
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre de la categoría es obligatorio",
                        "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtNombre.Focus();
                    return;
                }

                var categoria = new ModelCategoria
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    Estado = true
                };

                bool resultado;

                if (_modoEdicion && _categoriaSeleccionada != null)
                {
                    // Actualizar categoría existente
                    categoria.Id = _categoriaSeleccionada.Id;
                    resultado = _controller.ActualizarCategoria(categoria);

                    if (resultado)
                    {
                        MessageBox.Show("Categoría actualizada correctamente",
                            "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    // Crear nueva categoría
                    resultado = _controller.CrearCategoria(categoria);

                    if (resultado)
                    {
                        MessageBox.Show("Categoría creada correctamente",
                            "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                if (resultado)
                {
                    CargarCategorias();
                    MostrarTabla();
                    LimpiarFormulario();
                }
                else
                {
                    MessageBox.Show("Error al guardar la categoría",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Botón: Cancelar
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MostrarTabla();
            LimpiarFormulario();
            _modoEdicion = false;
            _categoriaSeleccionada = null;
        }

        // Limpiar formulario
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
        }

        // Búsqueda en tiempo real
        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = txtBuscar.Text;

            if (string.IsNullOrWhiteSpace(texto))
            {
                CargarCategorias();
                return;
            }

            try
            {
                var categorias = _controller.BuscarPorNombre(texto);
                dgCategorias.ItemsSource = categorias;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}