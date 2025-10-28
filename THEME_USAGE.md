# Guía de Uso de Temas, Estilos e Iconos

## Estructura de Archivos

```
Resources/
├── Themes/
│   ├── LightTheme.xaml    # Colores para tema claro
│   └── DarkTheme.xaml     # Colores para tema oscuro
├── Icons/                 # Archivos SVG originales
│   ├── home.svg
│   ├── book-text.svg
│   └── ...
├── Icons.xaml             # Iconos convertidos a WPF Geometry
└── Styles.xaml            # Estilos compartidos

Services/
└── ThemeService.cs        # Servicio para cambiar temas

Controls/
├── Icon.xaml              # Control reutilizable para iconos
└── Icon.xaml.cs
```

## Cambiar entre Temas

### En C# (Code-behind o ViewModel)

```csharp
using Enkarta.Services;

// Cambiar a tema oscuro
ThemeService.Instance.ChangeTheme(ThemeService.Theme.Dark);

// Cambiar a tema claro
ThemeService.Instance.ChangeTheme(ThemeService.Theme.Light);

// Alternar tema
ThemeService.Instance.ToggleTheme();
```

### Ejemplo de botón para cambiar tema:

```xaml
<Button Content="🌙 Cambiar Tema"
        Click="ToggleTheme_Click"
        Style="{StaticResource ButtonGhost}"/>
```

```csharp
private void ToggleTheme_Click(object sender, RoutedEventArgs e)
{
    ThemeService.Instance.ToggleTheme();
}
```

## Usar Colores en XAML

### Colores Disponibles

**Colores Base:**
- `BackgroundBrush` - Fondo general de la aplicación
- `PanelBrush` - Fondo de paneles y cards
- `TextBrush` - Texto principal
- `MutedTextBrush` - Texto secundario
- `LineBrush` - Bordes y líneas

**Colores de Acento:**
- `RedBrush` - Rojo navideño
- `GreenBrush` - Verde navideño
- `GoldBrush` - Dorado navideño

**Gradientes:**
- `SidebarGradientBrush` - Gradiente del sidebar
- `PrimaryGradientBrush` - Gradiente rojo-dorado
- `GreenGradientBrush` - Gradiente verde
- `LogoGradientBrush` - Gradiente del logo

### Ejemplo de uso:

```xaml
<!-- Usar brush directamente -->
<Border Background="{DynamicResource PanelBrush}"
        BorderBrush="{DynamicResource LineBrush}"
        BorderThickness="1">
    <TextBlock Text="Hola Mundo"
               Foreground="{DynamicResource TextBrush}"/>
</Border>
```

## Estilos de Texto

```xaml
<!-- Texto Principal -->
<TextBlock Text="Texto normal"
           Style="{StaticResource TextPrimary}"/>

<!-- Texto Secundario -->
<TextBlock Text="Texto secundario"
           Style="{StaticResource TextMuted}"/>

<!-- Título H1 -->
<TextBlock Text="Título Principal"
           Style="{StaticResource Heading1}"/>

<!-- Título H2 -->
<TextBlock Text="Subtítulo"
           Style="{StaticResource Heading2}"/>

<!-- KPI (números grandes) -->
<TextBlock Text="1,234"
           Style="{StaticResource KPI}"/>
```

## Estilos de Botones

```xaml
<!-- Botón Primary (Rojo-Dorado) -->
<Button Content="Guardar"
        Style="{StaticResource ButtonPrimary}"/>

<!-- Botón Green -->
<Button Content="Búsqueda"
        Style="{StaticResource ButtonGreen}"/>

<!-- Botón Soft (con borde) -->
<Button Content="Editar"
        Style="{StaticResource ButtonSoft}"/>

<!-- Botón Ghost (transparente) -->
<Button Content="Cancelar"
        Style="{StaticResource ButtonGhost}"/>

<!-- Botón de Menú (para sidebar) -->
<Button Content="📚 Artículos"
        Style="{StaticResource MenuButton}"/>
```

## Inputs

```xaml
<!-- TextBox -->
<TextBox Text="Escribir aquí..."
         Style="{StaticResource TextBoxBase}"/>

<!-- ComboBox -->
<ComboBox Style="{StaticResource ComboBoxBase}">
    <ComboBoxItem>Opción 1</ComboBoxItem>
    <ComboBoxItem>Opción 2</ComboBoxItem>
</ComboBox>
```

## Paneles y Cards

```xaml
<!-- Panel -->
<Border Style="{StaticResource Panel}">
    <TextBlock Text="Contenido del panel"/>
</Border>

<!-- Card -->
<Border Style="{StaticResource Card}">
    <StackPanel>
        <TextBlock Text="Título" Style="{StaticResource Heading2}"/>
        <TextBlock Text="Contenido" Style="{StaticResource TextPrimary}"/>
    </StackPanel>
</Border>
```

## Badges

```xaml
<Border Style="{StaticResource Badge}">
    <TextBlock Text="Publicado"
               Style="{StaticResource BadgeText}"/>
</Border>
```

## DataGrid (Tablas)

```xaml
<DataGrid Style="{StaticResource DataGridBase}"
          ItemsSource="{Binding Articulos}">
    <DataGrid.Columns>
        <DataGridTextColumn Header="Título" Binding="{Binding Titulo}"/>
        <DataGridTextColumn Header="Fecha" Binding="{Binding FechaPublicacion}"/>
    </DataGrid.Columns>
</DataGrid>
```

## Ejemplo Completo: Card con Datos

```xaml
<Border Style="{StaticResource Card}" Margin="10">
    <StackPanel>
        <!-- Icono decorativo -->
        <TextBlock Text="📚"
                   FontSize="32"
                   HorizontalAlignment="Right"
                   Opacity="0.3"/>

        <!-- Etiqueta -->
        <TextBlock Text="Artículos Totales"
                   Style="{StaticResource TextMuted}"/>

        <!-- KPI -->
        <TextBlock Text="32"
                   Style="{StaticResource KPI}"
                   Margin="0,8,0,0"/>

        <!-- Botón de acción -->
        <Button Content="Ver todos"
                Style="{StaticResource ButtonSoft}"
                Margin="0,12,0,0"/>
    </StackPanel>
</Border>
```

## Ejemplo Completo: Formulario

```xaml
<Border Style="{StaticResource Panel}">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <!-- Título -->
        <TextBlock Text="Nuevo Artículo"
                   Style="{StaticResource Heading2}"
                   Grid.Row="0"
                   Margin="0,0,0,20"/>

        <!-- Campo Título -->
        <StackPanel Grid.Row="1" Margin="0,0,0,12">
            <TextBlock Text="Título *"
                       Style="{StaticResource TextMuted}"
                       Margin="0,0,0,8"/>
            <TextBox Style="{StaticResource TextBoxBase}"/>
        </StackPanel>

        <!-- Campo Categoría -->
        <StackPanel Grid.Row="2" Margin="0,0,0,12">
            <TextBlock Text="Categoría *"
                       Style="{StaticResource TextMuted}"
                       Margin="0,0,0,8"/>
            <ComboBox Style="{StaticResource ComboBoxBase}">
                <ComboBoxItem>Cultura</ComboBoxItem>
                <ComboBoxItem>Historia</ComboBoxItem>
            </ComboBox>
        </StackPanel>

        <!-- Botones -->
        <StackPanel Grid.Row="3"
                    Orientation="Horizontal"
                    HorizontalAlignment="Right"
                    Margin="0,20,0,0">
            <Button Content="Cancelar"
                    Style="{StaticResource ButtonGhost}"
                    Margin="0,0,10,0"/>
            <Button Content="Guardar"
                    Style="{StaticResource ButtonPrimary}"/>
        </StackPanel>
    </Grid>
</Border>
```

## Iconos SVG

### Iconos Disponibles

- `IconHome` - Inicio/Home
- `IconBook` - Artículos/Libros
- `IconEdit` - Editar
- `IconFile` - Archivo/Documento
- `IconTag` - Etiqueta (una)
- `IconTags` - Etiquetas (múltiples)
- `IconLink` - Enlaces/Fuentes
- `IconImage` - Imagen/Medios
- `IconSearch` - Búsqueda
- `IconPackage` - Paquete/Exportar
- `IconBarChart` - Estadísticas/Gráficas
- `IconUser` - Usuario/Autores
- `IconSettings` - Configuración
- `IconBell` - Notificaciones
- `IconPlusCircle` - Agregar
- `IconSave` - Guardar
- `IconMoon` - Tema oscuro
- `IconSun` - Tema claro

### Usar iconos con el control Icon (Recomendado)

```xaml
<!-- Agregar namespace en la ventana -->
xmlns:controls="clr-namespace:Enkarta.Controls"

<!-- Usar el icono -->
<controls:Icon IconName="Home"
               IconSize="24"
               IconColor="{DynamicResource TextBrush}"/>

<!-- Icono con color personalizado -->
<controls:Icon IconName="Book"
               IconSize="32"
               IconColor="{DynamicResource RedBrush}"/>

<!-- Icono pequeño -->
<controls:Icon IconName="Edit"
               IconSize="16"
               IconColor="{DynamicResource MutedTextBrush}"/>
```

### Usar iconos directamente con Path

```xaml
<Path Data="{StaticResource IconHome}"
      Stroke="{DynamicResource TextBrush}"
      StrokeThickness="2"
      StrokeStartLineCap="Round"
      StrokeEndLineCap="Round"
      StrokeLineJoin="Round"
      Fill="Transparent"
      Width="24"
      Height="24"
      Stretch="Uniform"/>
```

### Ejemplo: Botón con Icono

```xaml
<Button Style="{StaticResource ButtonPrimary}">
    <StackPanel Orientation="Horizontal">
        <controls:Icon IconName="Save"
                       IconSize="18"
                       IconColor="White"
                       Margin="0,0,8,0"/>
        <TextBlock Text="Guardar"
                   VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

### Ejemplo: Menú con Iconos

```xaml
<Button Style="{StaticResource MenuButton}">
    <StackPanel Orientation="Horizontal">
        <controls:Icon IconName="Book"
                       IconSize="20"
                       IconColor="{DynamicResource SidebarTextBrush}"
                       Margin="0,0,10,0"/>
        <TextBlock Text="Artículos"
                   VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

### Ejemplo: Card con Icono

```xaml
<Border Style="{StaticResource Card}">
    <Grid>
        <!-- Icono decorativo en la esquina -->
        <controls:Icon IconName="BarChart"
                       IconSize="48"
                       IconColor="{DynamicResource GoldBrush}"
                       Opacity="0.2"
                       HorizontalAlignment="Right"
                       VerticalAlignment="Top"/>

        <StackPanel>
            <TextBlock Text="Estadísticas"
                       Style="{StaticResource TextMuted}"/>
            <TextBlock Text="1,234"
                       Style="{StaticResource KPI}"
                       Margin="0,8,0,0"/>
        </StackPanel>
    </Grid>
</Border>
```

## Notas Importantes

1. **Usa `DynamicResource` para colores** que cambiarán con el tema:
   ```xaml
   <Border Background="{DynamicResource PanelBrush}"/>
   ```

2. **Usa `StaticResource` para estilos** que no cambiarán:
   ```xaml
   <Button Style="{StaticResource ButtonPrimary}"/>
   ```

3. **Los temas se cambian automáticamente** cuando usas `ThemeService.Instance.ChangeTheme()`

4. **Todos los colores están sincronizados** con el mockup HTML del proyecto
