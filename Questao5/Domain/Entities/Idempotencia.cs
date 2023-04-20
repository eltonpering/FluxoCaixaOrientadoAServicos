using System.ComponentModel.DataAnnotations;

namespace Questao5.Domain.Entities;

public class Idempotencia
{
    [Key]
    [MaxLength(37)]
    public string? ChaveIdempotencia { get; set; }

    [MaxLength(1000)]
    public string? Requisicao { get; set; }

    [MaxLength(1000)]
    public string? Resultado { get; set; }
}
