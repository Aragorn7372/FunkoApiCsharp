using CSharpFunctionalExtensions;
using FunkoApi.Dto;
using FunkoApi.Dto.Funkasos;
using FunkoApi.Error;

namespace FunkoApi.Service.Funkos;

public interface IFunkoService
{
    Task<List<FunkoResponseDto>> GetFunkosAsync();
    Task<Result<FunkoResponseDto,FunkoError>> GetFunkoAsync(long id);
    Task<Result<FunkoResponseDto,FunkoError>> SaveFunkoAsync( FunkoRequestDto request,IFormFile? file);
    Task<Result<FunkoResponseDto,FunkoError>> DeleteFunkoAsync(long id);
    Task<Result<FunkoResponseDto,FunkoError>> UpdateFunkoAsync(long id,FunkoRequestDto request,IFormFile? file);
    
}