using MediatR;

namespace Questao5.Application.Commands.Requests;

public class CreateMovimentoCommandRequest : IRequest<string>
{
    public string IdRequisicao { get; set; }
    public string IdContaCorrente { get; set; }
    public decimal Valor { get; set; }
    public char TipoMovimento { get; set; }
}
