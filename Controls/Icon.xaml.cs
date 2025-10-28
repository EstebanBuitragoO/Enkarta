using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Enkarta.Controls
{
    public partial class Icon : UserControl
    {
        public static readonly DependencyProperty IconNameProperty =
            DependencyProperty.Register(nameof(IconName), typeof(string), typeof(Icon),
                new PropertyMetadata(string.Empty, OnIconNameChanged));

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(Icon),
                new PropertyMetadata(24.0));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(Icon),
                new PropertyMetadata(Brushes.Black));

        public string IconName
        {
            get => (string)GetValue(IconNameProperty);
            set => SetValue(IconNameProperty, value);
        }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public Brush IconColor
        {
            get => (Brush)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        public Icon()
        {
            InitializeComponent();
        }

        private static void OnIconNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Icon icon)
            {
                icon.LoadIcon(e.NewValue as string);
            }
        }

        private void LoadIcon(string? iconName)
        {
            if (string.IsNullOrEmpty(iconName))
                return;

            // Buscar el icono en los recursos
            if (Application.Current.TryFindResource($"Icon{iconName}") is Geometry geometry)
            {
                IconPath.Data = geometry;
            }
        }
    }
}
