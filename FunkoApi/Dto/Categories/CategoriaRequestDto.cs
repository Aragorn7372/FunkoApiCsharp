using System.ComponentModel.DataAnnotations;

namespace FunkoApi.Dto.Categories;

public record CategoriaRequestDto
{
    [Required(ErrorMessage = "Ingrese un nombre valido de funko")]
    [StringLength(100,MinimumLength = 2, ErrorMessage = "Ingrese un nombre entre2 y 100 caracteres")]
    public string Nombre {get; set;}=string.Empty;
};