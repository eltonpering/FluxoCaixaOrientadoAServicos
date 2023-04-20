using MediatR;
using Questao5.Application.ViewModels;

namespace Questao5.Application.Commands.Requests;

public class GetMovimentacaoRequest : IRequest<IEnumerable<MovimentoViewModel>>
{
    public GetMovimentacaoRequest(int numero)
    {
        Numero = numero;
    }

    public int Numero { get; private set; }
}
