using MediatR;
using Questao5.Application.ViewModels;

namespace Questao5.Application.Commands.Requests;

public class GetSaldoRequest : IRequest<SaldoContaCorrenteViewModel>
{
    public GetSaldoRequest(string idContaCorrente)
    {
        IdContaCorrente = idContaCorrente;
    }

    public string IdContaCorrente { get; private set; }
}
