using CSharpFunctionalExtensions;
using FunkoApi.Dto.Funkasos;
using FunkoApi.Error;
using FunkoApi.Handler.Funkos;
using FunkoApi.mapper;
using FunkoApi.Models;
using FunkoApi.Repository.Category;
using FunkoApi.Repository.funkos;
using FunkoApi.Service.Cache;
using FunkoApi.Service.storage;

namespace FunkoApi.Service.Funkos;

public class FunkoFunkoService(ICacheService cache, 
    IFunkoRepository repository,
    ICategoryRepository categoryRepository,
    IStorageService storage,
    FunkosWebSocketHandler webSocket,
    ILogger<FunkoFunkoService> logger)
    : IFunkoService
{
    private const string CacheKey = "Funko_";
  

    public async Task<List<FunkoResponseDto>> GetFunkosAsync()
    {
        logger.LogInformation("obtener funkos");
        return await Task.FromResult(repository.GetAllAsync().Result.Select(it => it.ToDto()).ToList());
    }

    public async Task<Result<FunkoResponseDto, FunkoError>> GetFunkoAsync(long id)
    {
          logger.LogInformation("obtener funko con id: " + id);
          return await cache.GetAsync<Funko>(CacheKey + id) is { } model
              ? Result.Success<FunkoResponseDto, FunkoError>(model.ToDto()).Tap(_=>
                  logger.LogInformation("funko obtenido de la cache se devuelve")
                  )
            : await repository.GetByIdAsync(id) is { } repoModel
                ? Result.Success<FunkoResponseDto, FunkoError>(repoModel.ToDto())
                    .Tap(_=>
                    {
                        cache.SetAsync(CacheKey + id, repoModel, TimeSpan.FromMinutes(30));
                        logger.LogInformation("funko obtenido y guardado en la cache con con id: " + repoModel.Id);
                    }) 
                : Result.Failure<FunkoResponseDto,FunkoError>(new FunkoNotFoundError("funko no encontrado con id: " + id))
                    .TapError(_=> logger.LogWarning("funko no encontrado con id: " + id));
    }


    public async Task<Result<FunkoResponseDto, FunkoError>> SaveFunkoAsync(FunkoRequestDto request,IFormFile? file)
    {
        
        var validationResult = await Valida(request);
        var image = await SaveImage(file);
        if (image.IsSuccess && image.Value != string.Empty)
        {
            request.Image=image.Value;
        }
        return validationResult.IsSuccess 
            ? image.IsSuccess
                ? await repository.AddAsync(request.ToModel(validationResult.Value)) is { } model
                    ? Result.Success<FunkoResponseDto, FunkoError>(
                         model.ToDto()
                    ).Tap(_=>
                    {
                        logger.LogInformation("funko guardado en la base de datos con id:" + model.Id);
                        NotificarWebSocketFunko(model.ToDto(), FunkoNotificationType.Created);
                    })
                    : Result.Failure<FunkoResponseDto, FunkoError>(
                        new FunkoError("no se pudo guardar el funko")
                    ).TapError(_=>logger.LogError("funko no ha sido guardado en la base de datos"))
                : Result.Failure<FunkoResponseDto, FunkoError>(image.Error)
            : Result.Failure<FunkoResponseDto, FunkoError>(validationResult.Error);
    }

    public async Task<Result<FunkoResponseDto, FunkoError>> DeleteFunkoAsync(long id)
    {
        
        return await repository.DeleteAsync(id) is { } model
            ? Result.Success<FunkoResponseDto, FunkoError>(model.ToDto()).Tap(_=>
            {
                logger.LogInformation("funko deleto con id:" + id);
                cache.RemoveAsync(CacheKey + id);
                NotificarWebSocketFunko(model.ToDto(), FunkoNotificationType.Deleted);
            })
            : Result.Failure<FunkoResponseDto, FunkoError>(new FunkoNotFoundError("no se encontro funko con id " + id))
                .TapError(_=> logger.LogWarning("funko no ha sido encontro funko con id: " + id));
    }

    public async Task<Result<FunkoResponseDto, FunkoError>> UpdateFunkoAsync(long id, FunkoRequestDto request,IFormFile? file)
    {   
        var validationResult = await Valida(request);
        var image = await SaveImage(file);
        if (image.IsSuccess && image.Value != string.Empty)
        {
            request.Image=image.Value;
        }
        return validationResult.IsSuccess
        ?image.IsSuccess
            ? await repository.UpdateAsync(id, request.ToModel(validationResult.Value)) is { } updateModel 
            ? Result.Success<FunkoResponseDto, FunkoError>(updateModel.ToDto())
                .Tap(_=>
                {
                    logger.LogInformation("funko valido y correctamente actualizado");
                    NotificarWebSocketFunko(updateModel.ToDto(), FunkoNotificationType.Updated);
                })
            : Result.Failure<FunkoResponseDto, FunkoError>(new FunkoNotFoundError("no se pudo guardar el funko con id:" + id))
                .TapError(_=> logger.LogWarning("funko no encontrado con id:" + id))
            : Result.Failure<FunkoResponseDto,FunkoError>(image.Error)
                .TapError(_=> logger.LogWarning("funko image no ha sido guardada"))
            : Result.Failure<FunkoResponseDto,FunkoError>(validationResult.Error)
                .TapError(_=> logger.LogWarning("funko invalido"));
    }

    private async Task<Result<Categoria,FunkoError>> Valida(FunkoRequestDto request)
    {
        return await categoryRepository.GetByIdAsync(request.Categoria) is { } categoria
            ? Result.Success<Categoria,FunkoError>(categoria)
                .Tap(_=> logger.LogInformation("funko valido"))
            : Result.Failure<Categoria,FunkoError>(new FunkoValidationError("funko no valido categoria no existe")
            ).TapError(_=> logger.LogWarning("funko no ha sido valido"));
    }

    private async Task<Result<string, FunkoError>> SaveImage(IFormFile? file)
    {
        try
        {
            return file is not null
                ? Result.Success<string,FunkoError>(await storage.StoreAsync(file))
                : Result.Success<string,FunkoError>(string.Empty);
        }
        catch (Exception e)
        {
            return Result.Failure<string, FunkoError>(new FunkoStorageError(e.Message));
        }
    }

    private void NotificarWebSocketFunko(FunkoResponseDto funko, string type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        _ = Task.Run(async () =>
        {
            try
            {
                await webSocket.NotifyAsync(new FunkoNotificacion(
                    type,
                    funko.Id,
                    funko
                ));
                logger.LogDebug("Notificación WebSocket enviada tras crear Funko: {FunkoId}", funko.Id);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error en notificación WebSocket al crear Funko: {FunkoId}", funko.Id);
            }
        });
    }
}