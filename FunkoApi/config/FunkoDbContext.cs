using FunkoApi.Models;
using Microsoft. EntityFrameworkCore;

namespace FunkoApi.config;

public class FunkoDbContext : DbContext
{
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SeedData(modelBuilder); // Llamamos al metodo para poblar la BD
    }
    
    // 1. DbSet<T> - Representan las tablas
    public DbSet<Funko> Funkos { get; set; } = null!;
    public DbSet<Categoria> Categorias { get; set; } = null!;
    
    // 2. Constructor con opciones
    public FunkoDbContext(DbContextOptions<FunkoDbContext> options)
        : base(options) { }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Define category IDs
        var categoriaAnimeId = Guid.NewGuid();
        var categoriaPeliculasId = Guid.NewGuid();
        var categoriaVideojuegosId = Guid.NewGuid();

        // Seed categories first
        var categoria1 = new Categoria
        {
            Id = categoriaAnimeId,
            Nombre = "Anime"
        };

        var categoria2 = new Categoria
        {
            Id = categoriaPeliculasId,
            Nombre = "Películas"
        };

        var categoria3 = new Categoria
        {
            Id = categoriaVideojuegosId,
            Nombre = "Videojuegos"
        };

        modelBuilder.Entity<Categoria>().HasData(categoria1, categoria2, categoria3);

        // Seed funkos using CategoryId (foreign key), not Category navigation property
        var funko1 = new Funko
        {
            Id = 1,
            Name = "Goku Super Saiyan",
            CategoryId = categoriaAnimeId,  // Use foreign key
            Imagen = "goku.png",
            Price = 19.99,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var funko2 = new Funko
        {
            Id = 2,
            Name = "Darth Vader",
            CategoryId = categoriaPeliculasId,  // Use foreign key
            Imagen = "vader.png",
            Price = 24.50,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var funko3 = new Funko
        {
            Id = 3,
            Name = "Mario Bros",
            CategoryId = categoriaVideojuegosId,  // Use foreign key
            Imagen = Funko.IMAGE_DEFAULT,
            Price = 17.75,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime. Now
        };

        modelBuilder.Entity<Funko>().HasData(funko1, funko2, funko3);
    }
}