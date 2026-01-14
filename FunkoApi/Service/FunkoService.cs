using CSharpFunctionalExtensions;
using FunkoApi.dto;
using FunkoApi.Error;
using FunkoApi.Repository;
using Microsoft.Extensions.Caching.Memory;
using FunkoApi.mapper;
using FunkoApi.Models;
using NLog;

namespace FunkoApi.Service;

public class FunkoService(IMemoryCache cache, FunkoRepository repository, CategoryRepository categoryRepository)
    : IService
{
    private const string CacheKey = "Funko_";
    private readonly Logger _logger=LogManager.GetCurrentClassLogger();

    public async Task<List<FunkoResponseDto>> GetFunkosAsync()
    {
        _logger.Info("obtener funkos");
        return await Task.FromResult(repository.GetAllAsync().Result.Select(it => it.ToDto()).ToList());
    }

    public async Task<Result<FunkoResponseDto, FunkoError>> GetFunkoAsync(long id)
    {
          _logger.Info("obtener funko con id: " + id);
          return cache.TryGetValue(CacheKey + id, out Funko? model)
              ? Result.Success<FunkoResponseDto, FunkoError>(model!.ToDto()).Tap(_=>
                  _logger.Info("funko obtenido de la cache se devuelve")
                  )
            : await repository.GetByIdAsync(id) is { } repoModel
                ? Result.Success<FunkoResponseDto, FunkoError>(
                    cache.Set(
                        CacheKey + id, repoModel, TimeSpan.FromMinutes(30)
                        ).ToDto()
                    ).Tap(_=>
                    _logger.Info("funko obtenido y guardado en la cache con con id: " + repoModel.Id)
                    ) 
                : Result.Failure<FunkoResponseDto,FunkoError>(new FunkoNotFoundError("funko no encontrado con id: " + id))
                    .TapError(_=> _logger.Warn("funko no encontrado con id: " + id));
    }


    public async Task<Result<FunkoResponseDto, FunkoError>> SaveFunkoAsync(FunkoRequestDto request)
    {
        return Valida(request).Result.TryGetValue(out var categoria)?
            await repository.AddAsync(request.ToModel(categoria)) is { } model
            ? Result.Success<FunkoResponseDto, FunkoError>(
                model.ToDto()
                ).Tap(_=> _logger.Info("funko guardado en la base de datos con id:"+model.Id))
            : Result.Failure<FunkoResponseDto, FunkoError>(
                new FunkoError("no se pudo guardar el funko")
                ).TapError(_=>_logger.Error("funko no ha sido guardado en la base de datos"))
            : Result.Failure<FunkoResponseDto, FunkoError>(new FunkoValidationError("funko no ha sido valido"));
    }

    public async Task<Result<FunkoResponseDto, FunkoError>> DeleteFunkoAsync(long id)
    {
        
        return await repository.DeleteAsync(id) is { } model
            ? Result.Success<FunkoResponseDto, FunkoError>(model.ToDto()).Tap(_=>
            {
                _logger.Info("funko deleto con id:" + id);
                cache.Remove(CacheKey + id);
            })
            : Result.Failure<FunkoResponseDto, FunkoError>(new FunkoNotFoundError("no se encontro funko con id " + id))
                .TapError(_=> _logger.Warn("funko no ha sido encontro funko con id: " + id));
    }

    public async Task<Result<FunkoResponseDto, FunkoError>> UpdateFunkoAsync(long id, FunkoRequestDto request)
    {
        return Valida(request).Result.TryGetValue(out var categoria)
            ? await repository.UpdateAsync(id, request.ToModel(categoria)) is { } updateModel 
            ? Result.Success<FunkoResponseDto, FunkoError>(updateModel.ToDto())
                .Tap(_=>_logger.Info("funko valido y correctamente actualizado"))
            : Result.Failure<FunkoResponseDto, FunkoError>(new FunkoNotFoundError("no se pudo guardar el funko con id:" + id))
                .TapError(_=> _logger.Warn("funko no encontrado con id:" + id))
            : Result.Failure<FunkoResponseDto,FunkoError>(new FunkoValidationError("funko no ha sido valido"))
                .TapError(_=> _logger.Warn("funko no ha sido valido"));
    }

    private async Task<Result<Categoria,FunkoError>> Valida(FunkoRequestDto request)
    {
        return await categoryRepository.GetByIdAsync(request.Categoria) is { } categoria
            ? Result.Success<Categoria,FunkoError>(categoria)
                .Tap(_=> _logger.Info("funko valido"))
            : Result.Failure<Categoria,FunkoError>(new FunkoValidationError("funko no valido categoria no existe")
            ).TapError(_=> _logger.Warn("funko no ha sido valido"));
    }
}