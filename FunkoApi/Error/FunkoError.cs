namespace FunkoApi.Error;

public record FunkoError(
    string Error)
{
    public string Error { get; set; } = Error;
};
public record FunkoNotFoundError(string Error) : FunkoError(Error);
public record FunkoBadRequestError(string Error) : FunkoError(Error);
public record FunkoValidationError(string Error) : FunkoError(Error);
public record FunkoStorageError(string Error) : FunkoError(Error);