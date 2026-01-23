using System.ComponentModel.DataAnnotations;

namespace FunkoApi.Dto.Funkasos;

public class FunkoPatchRequestDto
{
    [StringLength(100,MinimumLength = 2, ErrorMessage = "Ingrese un nombre entre2 y 100 caracteres")]
    public string Nombre {get; set;}=string.Empty;
    [Range(0.01,9999.9,ErrorMessage = "Ingrese un precio valido de funko")]
    public double Price  {get; set;}
    public string Categoria  {get; set;}= string.Empty;
    public string? Image { get; set; }
}