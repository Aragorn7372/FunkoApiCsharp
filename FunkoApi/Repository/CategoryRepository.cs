using FunkoApi.config;
using FunkoApi.Models;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace FunkoApi.Repository;

public class CategoryRepository(FunkoDbContext context) : IRepository<Categoria,string>
{
    private static Logger _log= LogManager.GetCurrentClassLogger();

    public async Task<List<Categoria>> GetAllAsync()
    {
        _log.Info("Getting all categorias");
        return await context.Categorias.ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(string id)
    {
        _log.Info("gettin categorie with id {id}", id);
        return await context.Categorias.FindAsync(id);
    }
}