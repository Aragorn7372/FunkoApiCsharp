using FunkoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FunkoApi.config;

public class FunkoDbContext : DbContext
{
    
    // 1. DbSet<T> - Representan las tablas
    public DbSet<Funko> Funkos { get; set; } = null!;
    public DbSet<Categoria> Categorias { get; set; } = null!;
    
    // 2. Constructor con opciones
    public FunkoDbContext(DbContextOptions<FunkoDbContext> options)
        : base(options) { }
    
}