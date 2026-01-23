using CSharpFunctionalExtensions;
using FunkoApi.Dto.Categories;
using FunkoApi.Error;

namespace FunkoApi.Service.Category;

public interface ICategoriaService
{
    Task<List<CategoriaResponseDto>> GetCategoriasAsync();
    Task<Result<CategoriaResponseDto,CategoriaError>> GetCategoriaAsync(string id);
    Task<Result<CategoriaResponseDto,CategoriaError>> SaveCategoriaAsync( CategoriaRequestDto request);
    Task<Result<CategoriaResponseDto,CategoriaError>> DeleteCategoriaAsync(Guid id);
    Task<Result<CategoriaResponseDto,CategoriaError>> UpdateCategoriaAsync(Guid id,CategoriaRequestDto request);
}