using System.ComponentModel.DataAnnotations;

namespace FunkoApi.dto;

public record FunkoRequestDto(
    string nombre,
    double precio,
    string categoria)
{
    [Required(ErrorMessage = "Ingrese un nombre valido de funko")]
    [StringLength(100,MinimumLength = 2, ErrorMessage = "Ingrese un nombre entre2 y 100 caracteres")]
    public string Nombre { get; private set; } = nombre;
    [Required(ErrorMessage = "Ingrese un precio valido de funko")]
    [Range(0.01,9999.9,ErrorMessage = "Ingrese un precio valido de funko")]
    public double Price { get; private set; } = precio;
    [Required(ErrorMessage = "Ingrese categoria valido de funko")]
    public string Categoria { get; private set; } = categoria;
};