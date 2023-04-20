namespace Questao5.Application.ViewModels;

public class MovimentoViewModel
{   
    public string IdMovimento { get; set; }
    public string IdContaCorrente { get; set; }
    public DateTime DataMovimento { get; set; }
    public char TipoMovimento { get; set; }
    public decimal Valor { get; set; }
}
