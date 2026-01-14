using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunkoApi.Models;


public record Categoria()
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Column]
    [Required]
    public string Nombre { get; set; }= string.Empty;
}