using FunkoApi.Models;

namespace FunkoApi.Repository.Category;

public interface ICategoryRepository : IRepository<Categoria,string>
{
    Task<Categoria?> UpdateAsync(Guid id, Categoria categoria);

    Task<Categoria?> DeleteAsync(Guid id);
}