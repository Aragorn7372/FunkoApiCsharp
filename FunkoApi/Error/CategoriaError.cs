namespace FunkoApi.Error;

public record CategoriaError(   
    string Error)
{
public string Error { get; set; } = Error;
};
public record CategoriaNotFoundError(string Error) : CategoriaError(Error);
public record CategoriaBadRequestError(string Error) : CategoriaError(Error);
public record CategoriaValidationError(string Error) : CategoriaError(Error);
public record CategoriaStorageError(string Error) : CategoriaError(Error);