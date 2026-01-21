using System.ComponentModel.DataAnnotations;

namespace FunkoApi.dto;

public record FunkoRequestDto()
{
    [Required(ErrorMessage = "Ingrese un nombre valido de funko")]
    [StringLength(100,MinimumLength = 2, ErrorMessage = "Ingrese un nombre entre2 y 100 caracteres")]
    public string Nombre {get; set;}=string.Empty;
    [Required(ErrorMessage = "Ingrese un precio valido de funko")]
    [Range(0.01,9999.9,ErrorMessage = "Ingrese un precio valido de funko")]
    public double Price  {get; set;}
    [Required(ErrorMessage = "Ingrese categoria valido de funko")]
    public string Categoria  {get; set;}= string.Empty;

    public string? Image { get; set; }
};