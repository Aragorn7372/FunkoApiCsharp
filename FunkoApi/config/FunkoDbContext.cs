using FunkoApi.Data;
using FunkoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FunkoApi.config;
/// <summary>
/// clase de configuracion de la base de datos donde se indican las tablas y columnas
/// </summary>
public class FunkoDbContext : DbContext
{
    /// <summary>
    /// genera tablas y columnas
    /// </summary>
    /// <param name="modelBuilder">generador de net para tablas y columnas</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SeedData(modelBuilder); 
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
            entity.Property(u => u.IsDeleted).HasDefaultValue(false);
            entity.ConfigureTimestamps();
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasQueryFilter(u => !u.IsDeleted);
        });
    }
    
    
    public DbSet<Funko> Funkos { get; set; } = null!;
    public DbSet<Categoria> Categorias { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    
  /// <summary>
  /// contructor con parametros
  /// </summary>
  /// <param name="options">opcionas para configurar la clase</param>
    public FunkoDbContext(DbContextOptions<FunkoDbContext> options)
        : base(options)
    { }
   
        
/// <summary>
/// inicializador de datos de ejemplo de la base de datos
/// </summary>
/// <param name="modelBuilder">controlador de la base de datos</param>
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
            CategoryId = categoriaAnimeId,  
            Imagen = "goku.png",
            Price = 19.99,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var funko2 = new Funko
        {
            Id = 2,
            Name = "Darth Vader",
            CategoryId = categoriaPeliculasId, 
            Imagen = "vader.png",
            Price = 24.50,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var funko3 = new Funko
        {
            Id = 3,
            Name = "Mario Bros",
            CategoryId = categoriaVideojuegosId,  
            Imagen = Funko.IMAGE_DEFAULT,
            Price = 17.75,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime. Now
        };

        modelBuilder.Entity<Funko>().HasData(funko1, funko2, funko3);
    }
    
}