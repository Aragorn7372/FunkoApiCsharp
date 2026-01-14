namespace FunkoApi.dto;

public record FunkoResponseDto(
    long id,
    string nombre,
    double precio,
    string categoria)
{
    public long Id { get; set; } = id;
    public string Nombre { get; set; } = nombre;
    public double Precio { get; set; } = precio;
    public string Categoria { get; set; } = categoria;
};