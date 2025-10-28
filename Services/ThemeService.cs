using System;
using System.Windows;

namespace Enkarta.Services
{
    public class ThemeService
    {
        private static ThemeService? _instance;
        private const string LIGHT_THEME_PATH = "Resources/Themes/LightTheme.xaml";
        private const string DARK_THEME_PATH = "Resources/Themes/DarkTheme.xaml";

        public static ThemeService Instance => _instance ??= new ThemeService();

        public enum Theme
        {
            Light,
            Dark
        }

        private Theme _currentTheme = Theme.Light;

        public Theme CurrentTheme
        {
            get => _currentTheme;
            private set => _currentTheme = value;
        }

        private ThemeService()
        {
        }

        /// <summary>
        /// Cambia el tema de la aplicación
        /// </summary>
        /// <param name="theme">Tema a aplicar (Light o Dark)</param>
        public void ChangeTheme(Theme theme)
        {
            var app = Application.Current;
            if (app == null) return;

            // Obtener los recursos combinados
            var dictionaries = app.Resources.MergedDictionaries;
            if (dictionaries.Count == 0) return;

            // Determinar la ruta del tema
            string themePath = theme == Theme.Light ? LIGHT_THEME_PATH : DARK_THEME_PATH;

            // Crear el nuevo diccionario de recursos
            var newTheme = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            };

            // Reemplazar el primer diccionario (que es el tema)
            dictionaries[0] = newTheme;

            CurrentTheme = theme;
        }

        /// <summary>
        /// Alterna entre tema claro y oscuro
        /// </summary>
        public void ToggleTheme()
        {
            var newTheme = CurrentTheme == Theme.Light ? Theme.Dark : Theme.Light;
            ChangeTheme(newTheme);
        }

        /// <summary>
        /// Carga el tema guardado desde la configuración (implementar persistencia)
        /// </summary>
        public void LoadSavedTheme()
        {
            // TODO: Implementar carga desde configuración/settings
            // Por ahora, siempre carga el tema claro
            ChangeTheme(Theme.Light);
        }

        /// <summary>
        /// Guarda el tema actual en la configuración (implementar persistencia)
        /// </summary>
        public void SaveTheme()
        {
            // TODO: Implementar guardado en configuración/settings
        }
    }
}
