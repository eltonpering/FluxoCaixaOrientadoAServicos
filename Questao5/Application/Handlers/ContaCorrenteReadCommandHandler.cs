using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Handlers;

public class ContaCorrenteReadCommandHandler : IRequestHandler<GetContaCorrenteCommandRequest, IEnumerable<ContaCorrente>>
{
    private readonly IContaCorrenteRepository _accountRepository;

    public ContaCorrenteReadCommandHandler(IContaCorrenteRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<IEnumerable<ContaCorrente>> Handle(GetContaCorrenteCommandRequest request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAllAsync();           

        return account;
    }
}
