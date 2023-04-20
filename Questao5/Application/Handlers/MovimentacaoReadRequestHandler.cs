using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Interfaces;
using Questao5.Application.ViewModels;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Handlers;

public class MovimentacaoReadRequestHandler : IRequestHandler<GetMovimentacaoRequest, IEnumerable<MovimentoViewModel>>
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IServiceContext _serviceContext;

    public MovimentacaoReadRequestHandler(
        IContaCorrenteRepository accountRepository,
        IMovimentoRepository movementRepository,
        IServiceContext serviceContext)
    {
        _contaCorrenteRepository = accountRepository;
        _movimentoRepository = movementRepository;
        _serviceContext = serviceContext;
    }

    public async Task<IEnumerable<MovimentoViewModel>> Handle(GetMovimentacaoRequest request, CancellationToken cancellationToken)
    {
        // Verifica pelo número se a conta corrente existe
        var accountExists = await _contaCorrenteRepository.ContaCorrenteExisteByNumeroAsync(request.Numero);
        if (!accountExists)
        {
            _serviceContext.AddError("Conta corrente não encontrada.");
            return default;
        }

        // Busca a movimentação pelo número da conta corrente
        var result = await _movimentoRepository.GetContaCorrenteByNumeroAsync(request.Numero);

        return result;
    }
}
