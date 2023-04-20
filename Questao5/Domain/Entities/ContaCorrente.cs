using System.ComponentModel.DataAnnotations;

namespace Questao5.Domain.Entities;

public class ContaCorrente
{
    [Key]
    [MaxLength(37)]
    public string IdContaCorrente { get; set; } 

    [Required]
    public int Numero { get; set; } 

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } 
    
    [Required]
    public bool Ativo { get; set; } 

    public ICollection<Movimento> Movimentos { get; set; } 
}
