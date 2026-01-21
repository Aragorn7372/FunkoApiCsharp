using FunkoApi.config;
using FunkoApi.Models;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace FunkoApi.Repository.funkos;

public class FunkoRepository(FunkoDbContext context) : IFunkoRepository
{
    private static Logger _log= LogManager.GetCurrentClassLogger();
    
    public async Task<List<Funko>> GetAllAsync()
    {
        _log.Info("Getting all Funkos");
        return await  context.Funkos
            .Include(f => f.Category)
            .ToListAsync();
    }

    public async Task<Funko?> GetByIdAsync(long id)
    {
        _log.Info("Getting Funko with id: " + id);
        return await context. Funkos
            .Include(f => f.Category)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Funko?> UpdateAsync(long id, Funko newFunko)
    {
        _log.Info("Updating Funko with id: " + id);
        newFunko.Id = id;
        var found=await context.Funkos.FindAsync(id);
        if (found != null)
        {
            found.Name = newFunko.Name;
            found.Category = newFunko.Category;
            found.Price= newFunko.Price;
            found.UpdatedAt= DateTime.Now;
            var updated =  context.Funkos.Update(found);
            await context.SaveChangesAsync();
            await context.Funkos.Entry(found).Reference(f => f.Category).LoadAsync();
            return updated.Entity;
        }
        return null;
    }

    public async Task<Funko> AddAsync(Funko newFunko)
    { 
        _log.Info("Adding Funko");
        var saved=await context.Funkos.AddAsync(newFunko);
        await context.SaveChangesAsync();
        await context.Funkos.Entry(newFunko).Reference(f => f.Category).LoadAsync();
        return saved.Entity;
    }

    public async Task<Funko?> DeleteAsync(long id)
    {
        _log.Info("Deleting Funko with id: " + id);
        var deleted=await context. Funkos
            .Include(f => f.Category)
            .FirstOrDefaultAsync(f => f.Id.Equals(id)) is { } funko
            ? context.Funkos.Remove(funko).Entity
            : null;
        await context.SaveChangesAsync();
        return deleted;
    }

   
}