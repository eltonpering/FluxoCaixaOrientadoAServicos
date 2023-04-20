using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Interfaces;
using Questao5.Application.ViewModels;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Commands.Handlers;

public class CreateMovimentoCommandHandler : IRequestHandler<CreateMovimentoCommandRequest, string>
{
    
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IIdempotenciaRepository _idempotenciaRepository;
    private readonly IServiceContext _serviceContext;

    public CreateMovimentoCommandHandler(        
        IContaCorrenteRepository accountRepository,
        IMovimentoRepository movementRepository,
        IIdempotenciaRepository idempotenciaRepository,
        IServiceContext serviceContext)
    {
        _contaCorrenteRepository = accountRepository;
        _movimentoRepository = movementRepository;
        _idempotenciaRepository = idempotenciaRepository;
        _serviceContext = serviceContext;
    }

    public async Task<string> Handle(CreateMovimentoCommandRequest request, CancellationToken cancellationToken)
    {
        // Verificar se a requisição já foi processada anteriormente
        if (!string.IsNullOrEmpty(request.IdRequisicao))
        {
            var idempotenciaRequisicao = await _idempotenciaRepository.GetIdempotencyKeyByRequestIdAsync(request.IdRequisicao);
            if (idempotenciaRequisicao != null)
            {
                // A requisição já foi processada anteriormente, retornar resultado previamente armazenado
                _serviceContext.AddError("Requisição já foi processada anteriormente: \n ." + idempotenciaRequisicao.Resultado);
                return default;
            }
        }

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

        // Verifica se o valor é positivo
        if (request.Valor <= 0)
        {
            _serviceContext.AddError("Valor inválido.");
            return default;
        }

        //Verifica se o tipo de movimento é válido
        if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D')
        {
            _serviceContext.AddError("Tipo de movimento inválido.");
            return default;
        }      

        // Cria a movimentação
        var movimento = new Movimento
        {
            IdMovimento = Guid.NewGuid().ToString(),
            IdContaCorrente = request.IdContaCorrente,
            DataMovimento = DateTime.Now,
            TipoMovimento = request.TipoMovimento,
            Valor = request.Valor
        };

        // Persiste a movimentação
        var idMovimento = await _movimentoRepository.AddMovimentoAsync(movimento);

        var result = new MovimentoViewModel()
        {
            IdMovimento = idMovimento,
            IdContaCorrente = request.IdContaCorrente,
            DataMovimento = DateTime.Now,
            Valor = request.Valor,
            TipoMovimento = request.TipoMovimento
        };

        // Salvar resultado para futuras consultas com a mesma chave de idempotência
        if (!string.IsNullOrEmpty(request.IdRequisicao))
        {
            var idempotencia = new Idempotencia
            {
                ChaveIdempotencia = request.IdRequisicao,
                Requisicao = JsonConvert.SerializeObject(request),
                Resultado = JsonConvert.SerializeObject(result)
            };

            await _idempotenciaRepository.AddIdempotencyKeyAsync(idempotencia);
        }

        return idMovimento;
    }    
}
