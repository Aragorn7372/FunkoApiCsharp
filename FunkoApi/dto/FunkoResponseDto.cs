namespace FunkoApi.dto;

public record FunkoResponseDto(
    long id,
    string nombre,
    double precio,
    string categoria,
    string imagen)
{
    public long Id  = id;
    public string Nombre  = nombre;
    public double Precio  = precio;
    public string Categoria = categoria;
    public string Imagen = imagen;
};