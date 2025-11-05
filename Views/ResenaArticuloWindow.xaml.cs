using Enkarta.Models;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Collections.Generic;

namespace Enkarta.Views
{
    public partial class ResenaArticuloWindow : Window
    {
        private readonly ModelArticulo _articulo;
        public ResenaArticuloWindow(ModelArticulo articulo)
        {
            InitializeComponent();
            _articulo = articulo;
            LoadData();
        }

        private void LoadData()
        {
            // Mostrar título y metadatos (decodificar entidades si las hubiera)
            txtTitulo.Text = WebUtility.HtmlDecode(_articulo.Titulo ?? "Sin título");
            txtCategoria.Text = WebUtility.HtmlDecode(_articulo.Categoria?.Nombre ?? "N/A");
            txtFecha.Text = _articulo.FechaPublicacion?.ToString("dd/MM/yyyy") ?? "N/A";

            // Generar siempre la reseña a partir del título (asegurar valor no nulo)
            string tituloLimpio = WebUtility.HtmlDecode(_articulo.Titulo ?? string.Empty) ?? string.Empty;
            txtResumen.Text = GenerarResenaDesdeTitulo(tituloLimpio);
        }

        private string GenerarResenaDesdeTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return "No hay título disponible para generar una reseña. Consulta la fuente para más detalles.";

            // Normalizar: eliminar caracteres no deseados y múltiples espacios
            var cleaned = Regex.Replace(titulo.Trim(), @"\s+", " ");
            cleaned = Regex.Replace(cleaned, @"[^\p{L}\p{N}\s\-]", string.Empty);

            // Diccionario simple de temas y frases descriptivas
            var temas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "navidad", "las tradiciones y celebraciones de la Navidad" },
                { "navideño", "las tradiciones y celebraciones de la Navidad" },
                { "cultura", "aspectos culturales" },
                { "historia", "aspectos históricos" },
                { "tecnología", "las últimas tendencias tecnológicas" },
                { "tecnologia", "las últimas tendencias tecnológicas" },
                { "salud", "temas de salud y bienestar" },
                { "educación", "la educación y sus prácticas" },
                { "educacion", "la educación y sus prácticas" },
                { "arte", "manifestaciones artísticas" },
                { "música", "música y sus influencias" },
                { "musica", "música y sus influencias" },
                { "deporte", "prácticas deportivas y eventos" },
                { "economía", "cuestiones económicas y financieras" },
                { "economia", "cuestiones económicas y financieras" }
            };

            // Buscar palabra clave en el título
            var palabras = cleaned
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim(new char[] { '.', ',', ';', ':', '"' }))
                .ToArray();

            string temaDetectado = null;
            foreach (var p in palabras)
            {
                if (temas.TryGetValue(p, out var descripcion))
                {
                    temaDetectado = descripcion;
                    break;
                }
            }

            // Construir reseña
            string frase1;
            if (!string.IsNullOrEmpty(temaDetectado))
            {
                frase1 = $"El artículo «{cleaned}» aborda {temaDetectado}.";
            }
            else
            {
                // Si no se detecta tema, extraer palabras clave significativas
                var stopwords = new HashSet<string>(new[] { "el","la","los","las","de","del","y","en","para","con","por","un","una","al","sobre","su" }, StringComparer.OrdinalIgnoreCase);
                var claves = palabras
                    .Where(w => w.Length > 2 && !stopwords.Contains(w))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Take(4)
                    .ToArray();

                if (claves.Length > 0)
                    frase1 = $"El artículo «{cleaned}» trata sobre {string.Join(", ", claves)}.";
                else
                    frase1 = $"Breve reseña basada en el título: «{cleaned}».";
            }

            var frase2 = "Ofrece una introducción clara y puntual sobre el tema.";
            var fraseCierre = "Consulta la fuente para más detalles.";

            var resumen = $"{frase1} {frase2} {fraseCierre}";

            // Asegurar longitud razonable
            if (resumen.Length > 800) resumen = resumen.Substring(0, 800) + "...";

            return resumen;
        }

        private void BtnIrFuentes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}