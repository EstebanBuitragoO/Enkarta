using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Enkarta.Services;

namespace Enkarta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button? _activeMenuButton;

        public MainWindow()
        {
            InitializeComponent();

            // Establecer el botón Inicio como activo por defecto
            SetActiveMenuButton(BtnInicio);
            NavigateToPage("Inicio");
        }

        /// <summary>
        /// Maneja el clic en los botones del menú
        /// </summary>
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                SetActiveMenuButton(button);

                // Obtener el tag del botón para saber a qué página navegar
                string? pageName = button.Tag as string;
                if (!string.IsNullOrEmpty(pageName))
                {
                    NavigateToPage(pageName);
                }
            }
        }

        /// <summary>
        /// Establece el botón activo en el menú
        /// </summary>
        private void SetActiveMenuButton(Button button)
        {
            // Quitar el estilo activo del botón anterior
            if (_activeMenuButton != null)
            {
                _activeMenuButton.Background = Brushes.Transparent;
            }

            // Establecer el nuevo botón activo
            _activeMenuButton = button;

            // Aplicar estilo activo (fondo semitransparente)
            var activeBrush = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255));
            button.Background = activeBrush;
        }

        /// <summary>
        /// Navega a la página especificada
        /// </summary>
        private void NavigateToPage(string pageName)
        {
            // Actualizar breadcrumb y título
            Breadcrumb.Text = $"Inicio › {pageName}";
            PageTitle.Text = GetPageTitle(pageName);

            // Limpiar el área de contenido
            ContentArea.Children.Clear();

            // Aquí se cargarán las vistas correspondientes
            // Por ahora, mostramos un mensaje temporal
            var tempMessage = new TextBlock
            {
                Text = $"Vista de {pageName} (próximamente)",
                Style = (Style)FindResource("Heading2"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = (Brush)FindResource("MutedTextBrush")
            };

            ContentArea.Children.Add(tempMessage);
        }

        /// <summary>
        /// Obtiene el título de la página basado en su nombre
        /// </summary>
        private string GetPageTitle(string pageName)
        {
            return pageName switch
            {
                "Inicio" => "Bienvenido a Enkarta",
                "Articulos" => "Gestión de Artículos",
                "Categorias" => "Gestión de Categorías",
                "Autores" => "Gestión de Autores",
                "Etiquetas" => "Gestión de Etiquetas",
                "Fuentes" => "Gestión de Fuentes",
                "Medios" => "Gestión de Medios",
                "Busqueda" => "Búsqueda Avanzada",
                "Exportar" => "Centro de Exportación",
                "Estadisticas" => "Estadísticas y Reportes",
                _ => pageName
            };
        }

        /// <summary>
        /// Alterna entre tema claro y oscuro
        /// </summary>
        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeService.Instance.ToggleTheme();

            // Actualizar el icono del botón
            if (ThemeService.Instance.CurrentTheme == ThemeService.Theme.Dark)
            {
                ThemeIcon.Data = (Geometry)FindResource("IconSun");
            }
            else
            {
                ThemeIcon.Data = (Geometry)FindResource("IconMoon");
            }
        }
    }
}