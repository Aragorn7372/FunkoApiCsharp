using FunkoApi.config;
using FunkoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FunkoApi.Repository.Category;

public class CategoryRepository(FunkoDbContext context,ILogger<CategoryRepository> log) : ICategoryRepository
{

    public async Task<List<Categoria>> GetAllAsync()
    {
        log.LogInformation("Getting all categorias");
        return await context.Categorias.ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(string id)
    {
        log.LogInformation("gettin categorie with id {id}", id);
        return await context.Categorias.Where(c=> c.Nombre == id).FirstOrDefaultAsync();
    }

    public async Task<Categoria?> UpdateAsync(Guid id, Categoria categoria)
    {
        log.LogInformation("Updating Categoria with id: " + id);
        var found=await context.Categorias.FindAsync(id);
        if (found != null)
        {
            found.Nombre = categoria.Nombre;
            var updated =  context.Categorias.Update(found);
            await context.SaveChangesAsync();
            return updated.Entity;
        }
        return null;
    }

    public async Task<Categoria> AddAsync(Categoria categoria)
    {
        log.LogInformation("Adding Funko");
        var saved=await context.Categorias.AddAsync(categoria);
        await context.SaveChangesAsync();
        return saved.Entity;
    }

    public async Task<Categoria?> DeleteAsync(Guid id)
    {
        log.LogInformation("Deleting Categoria with id: " + id);
        var deleted=await context.Categorias
            .FirstOrDefaultAsync(c => c.Id.Equals(id)) is { } categoria
            ? context.Categorias.Remove(categoria).Entity
            : null;
        await context.SaveChangesAsync();
        return deleted;
    }
}