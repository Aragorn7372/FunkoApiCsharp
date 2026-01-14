using FunkoApi.Models;

namespace FunkoApi.Repository;

public interface IFunkoRepository : IRepository<Funko,long>
{
    Task<Funko?> UpdateAsync(long id, Funko newFunko);
    Task<Funko> AddAsync(Funko newFunko);
    Task<Funko?> DeleteAsync(long id);
}