using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Interfaces;
using Questao5.Application.ViewModels;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Handlers;

public class SaldoContaCorrenteReadRequestHandler : IRequestHandler<GetSaldoRequest, SaldoContaCorrenteViewModel>
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IServiceContext _serviceContext;

    public SaldoContaCorrenteReadRequestHandler(
            IContaCorrenteRepository accountRepository,
            IMovimentoRepository movementRepository,
            IServiceContext serviceContext)
    {
        _contaCorrenteRepository = accountRepository;
        _movimentoRepository = movementRepository;
        _serviceContext = serviceContext;
    }

    public async Task<SaldoContaCorrenteViewModel> Handle(GetSaldoRequest request, CancellationToken cancellationToken)
    {
        // Verifica se a conta corrente existe e está ativa
        var accountExists = await _contaCorrenteRepository.ContaCorrenteExisteByIdAsync(request.IdContaCorrente);
        if (!accountExists)
        {
            _serviceContext.AddError("Conta corrente não encontrada.");
            return default;
        }

        var accountIsActive = await _contaCorrenteRepository.ContaCorrrenteAtivaByIdAsync(request.IdContaCorrente);
        if (!accountIsActive)
        {
            _serviceContext.AddError("Conta corrente inativa.");
            return default;
        }

        // Busca o saldo da conta corrente
        var result = await _movimentoRepository.GetSaldoContaCorrenteByIdAsync(request.IdContaCorrente);
        result.DataConsulta = DateTime.Now;

        return result;
    }
}
