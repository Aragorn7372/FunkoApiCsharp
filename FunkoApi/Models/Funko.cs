using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunkoApi.Models;

public record Funko(
    string Name,
    Categoria Category,
    double Price
    )
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Column]
    [Required]
    [StringLength(50)]
    public string Name { get; set; }= Name; 
    [Column]
    [Required]
    public Categoria? Category { get; set; }=Category;

    [Column]
    [Required]
    [Range(0, int.MaxValue)]
    public double Price { get; set; } = Price;
    [Column]
    [Required]
    public DateTime CreatedAt { get; set; }= DateTime.Now;
    [Column]
    [Required]
    public DateTime UpdatedAt { get; set; }= DateTime.Now;
}