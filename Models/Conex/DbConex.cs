using Microsoft.EntityFrameworkCore;
using System;

namespace Enkarta.Models.Conex
{
    public class DbConex : DbContext
    {
        // DbSets - Representan las tablas en la base de datos
        public DbSet<ModelArticulo> Articulos { get; set; }
        public DbSet<ModelCategoria> Categorias { get; set; }
        public DbSet<ModelAutor> Autores { get; set; }
        public DbSet<ModelEtiqueta> Etiquetas { get; set; }
        public DbSet<ModelFuente> Fuentes { get; set; }
        public DbSet<ModelMedio> Medios { get; set; }
        public DbSet<ModelArticuloAutor> ArticuloAutores { get; set; }
        public DbSet<ModelArticuloEtiqueta> ArticuloEtiquetas { get; set; }
        public DbSet<ModelArticuloFuente> ArticuloFuentes { get; set; }
        public DbSet<ModelArticuloMedio> ArticuloMedios { get; set; }

        // Constructor sin parámetros
        public DbConex()
        {
        }

        // Constructor con opciones (para inyección de dependencias)
        public DbConex(DbContextOptions<DbConex> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configuración para XAMPP por defecto (sin password)
                var connectionString = "server=localhost;database=enkarta;user=root;password=";
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));

                optionsBuilder.UseMySql(connectionString, serverVersion);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de nombres de tablas
            modelBuilder.Entity<ModelArticulo>().ToTable("Articulos");
            modelBuilder.Entity<ModelCategoria>().ToTable("Categorias");
            modelBuilder.Entity<ModelAutor>().ToTable("Autores");
            modelBuilder.Entity<ModelEtiqueta>().ToTable("Etiquetas");
            modelBuilder.Entity<ModelFuente>().ToTable("Fuentes");
            modelBuilder.Entity<ModelMedio>().ToTable("Medios");
            modelBuilder.Entity<ModelArticuloAutor>().ToTable("ArticuloAutores");
            modelBuilder.Entity<ModelArticuloEtiqueta>().ToTable("ArticuloEtiquetas");
            modelBuilder.Entity<ModelArticuloFuente>().ToTable("ArticuloFuentes");
            modelBuilder.Entity<ModelArticuloMedio>().ToTable("ArticuloMedios");

            // Configuración de relaciones

            // Articulo -> Categoria (muchos a uno)
            modelBuilder.Entity<ModelArticulo>()
                .HasOne(a => a.Categoria)
                .WithMany(c => c.Articulos)
                .HasForeignKey(a => a.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // ArticuloAutor - Relación muchos a muchos
            modelBuilder.Entity<ModelArticuloAutor>()
                .HasOne(aa => aa.Articulo)
                .WithMany(a => a.ArticuloAutores)
                .HasForeignKey(aa => aa.ArticuloId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ModelArticuloAutor>()
                .HasOne(aa => aa.Autor)
                .WithMany(a => a.ArticuloAutores)
                .HasForeignKey(aa => aa.AutorId)
                .OnDelete(DeleteBehavior.Cascade);

            // ArticuloEtiqueta - Relación muchos a muchos
            modelBuilder.Entity<ModelArticuloEtiqueta>()
                .HasOne(ae => ae.Articulo)
                .WithMany(a => a.ArticuloEtiquetas)
                .HasForeignKey(ae => ae.ArticuloId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ModelArticuloEtiqueta>()
                .HasOne(ae => ae.Etiqueta)
                .WithMany(e => e.ArticuloEtiquetas)
                .HasForeignKey(ae => ae.EtiquetaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ArticuloFuente - Relación muchos a muchos
            modelBuilder.Entity<ModelArticuloFuente>()
                .HasOne(af => af.Articulo)
                .WithMany(a => a.ArticuloFuentes)
                .HasForeignKey(af => af.ArticuloId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ModelArticuloFuente>()
                .HasOne(af => af.Fuente)
                .WithMany(f => f.ArticuloFuentes)
                .HasForeignKey(af => af.FuenteId)
                .OnDelete(DeleteBehavior.Cascade);

            // ArticuloMedio - Relación muchos a muchos
            modelBuilder.Entity<ModelArticuloMedio>()
                .HasOne(am => am.Articulo)
                .WithMany(a => a.ArticuloMedios)
                .HasForeignKey(am => am.ArticuloId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ModelArticuloMedio>()
                .HasOne(am => am.Medio)
                .WithMany(m => m.ArticuloMedios)
                .HasForeignKey(am => am.MedioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de valores por defecto
            modelBuilder.Entity<ModelArticulo>()
                .Property(a => a.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<ModelCategoria>()
                .Property(c => c.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<ModelAutor>()
                .Property(a => a.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<ModelEtiqueta>()
                .Property(e => e.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<ModelFuente>()
                .Property(f => f.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<ModelMedio>()
                .Property(m => m.Estado)
                .HasDefaultValue(true);

            // Configuración de timestamps automáticos
            modelBuilder.Entity<ModelArticulo>()
                .Property(a => a.FechaCreado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<ModelCategoria>()
                .Property(c => c.FechaCreado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<ModelAutor>()
                .Property(a => a.FechaCreado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<ModelEtiqueta>()
                .Property(e => e.FechaCreado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<ModelFuente>()
                .Property(f => f.FechaCreado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<ModelMedio>()
                .Property(m => m.FechaCreado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
