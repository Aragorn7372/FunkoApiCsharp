using CSharpFunctionalExtensions;
using FunkoApi.dto;
using FunkoApi.Error;
using FunkoApi.Models;

namespace FunkoApi.Service;

public interface IService
{
    Task<List<FunkoResponseDto>> GetFunkosAsync();
    Task<Result<FunkoResponseDto,FunkoError>> GetFunkoAsync(long id);
    Task<Result<FunkoResponseDto,FunkoError>> SaveFunkoAsync( FunkoRequestDto request,IFormFile? file);
    Task<Result<FunkoResponseDto,FunkoError>> DeleteFunkoAsync(long id);
    Task<Result<FunkoResponseDto,FunkoError>> UpdateFunkoAsync(long id,FunkoRequestDto request,IFormFile? file);
    
}