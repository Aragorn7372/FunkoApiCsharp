namespace FunkoApi.Error;

public record FunkoError(
    string Error)
{
    public string Error { get; set; } = Error;
};
public record FunkoNotFoundError(string error) : FunkoError(error);
public record FunkoBadRequestError(string error) : FunkoError(error);
public record FunkoValidationError(string error) : FunkoError(error);
public record FunkoStorageError(string error) : FunkoError(error);