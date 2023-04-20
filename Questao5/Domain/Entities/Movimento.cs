using System.ComponentModel.DataAnnotations;

namespace Questao5.Domain.Entities;

public class Movimento
{
    [Key]
    [MaxLength(37)]
    public string IdMovimento { get; set; } 
    
    [Required]
    public string IdContaCorrente { get; set; } 
    
    [Required]
    public DateTime DataMovimento { get; set; } 
    
    [Required]
    public char TipoMovimento { get; set; }
    
    [Required]
    public decimal Valor { get; set; } 

    public ContaCorrente ContaCorrente { get; set; } 
}
