namespace FunkoApi.Dto.Funkasos;

public record FunkoResponseDto(
    long Id,
    string Nombre,
    double Precio,
    string Categoria,
    string Imagen)
{
    public long Id  = Id;
    public string Nombre  = Nombre;
    public double Precio  = Precio;
    public string Categoria = Categoria;
    public string Imagen = Imagen;
};