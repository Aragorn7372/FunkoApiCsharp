namespace FunkoApi.Dto.Categories;

public record CategoriaResponseDto(
    Guid Id,
    string Nombre)
{
    public Guid Id { get; set; } = Id;
    public string Nombre { get; set; } = Nombre;
};