using FunkoApi.Models;

namespace FunkoApi.Repository.funkos;

public interface IFunkoRepository : IRepository<Funko,long>
{
    Task<Funko?> UpdateAsync(long id, Funko newFunko);
    Task<Funko?> DeleteAsync(long id);
}