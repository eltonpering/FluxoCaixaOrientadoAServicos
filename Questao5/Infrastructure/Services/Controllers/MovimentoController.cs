using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Interfaces;
using Questao5.Application.ViewModels;
using Questao5.Infrastructure.Services.Models;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[Route("[controller]")]
public class MovimentoController : PublicControllerBase
{
    public MovimentoController(IServiceContext serviceContext) : base(serviceContext)
    {
    }

    /// <summary>
    /// Cria uma movimentação da conta corrente
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Retorna a confirmação da movimentação e status 200 ok</returns>
    /// <response code="200">Movimentacao efetuada</response>
    [HttpPost("movimentacontacorrente")]
    [ProducesResponseType(typeof(MovimentoViewModel), 200)]
    [ProducesResponseType(typeof(BadRequestModel), 400)]
    public async Task<IActionResult> MovimentaContaCorrente([FromBody] CreateMovimentoCommandRequest request)
    {
        return GetResponse(await Mediator.Send(request));
    }

    /// <summary>
    /// Obtém a movimentação da conta pelo número da conta corrente.
    /// </summary>
    /// <param name="numeroContaCorrente"></param>
    /// <returns>Retorna a movimentação da conta e status 200 ok</returns>
    /// <response code="200">Movimentacao Listada</response>
    [HttpGet("listamovimentacaocontacorrente/{numeroContaCorrente}")]
    [ProducesResponseType(typeof(MovimentoViewModel), 200)]
    [ProducesResponseType(typeof(BadRequestModel), 400)]
    public async Task<IActionResult> GetMovimentacao(int numeroContaCorrente)
    {   
        return GetResponse(await Mediator.Send(new GetMovimentacaoRequest(numeroContaCorrente)));
    }

    /// <summary>
    /// Obter o saldo da conta pelo id da conta corrente
    /// </summary>
    /// <param name="idContaCorrente"></param>
    /// <returns>Retorna o saldo da conta e status 200 ok</returns>
    /// <response code="200">Saldo Obtido</response>
    [HttpGet("saldo/{idContaCorrente}")]
    [ProducesResponseType(typeof(SaldoContaCorrenteViewModel), 200)]
    [ProducesResponseType(typeof(BadRequestModel), 400)]
    public async Task<IActionResult> GetSaldo(string idContaCorrente)
    {   
        return GetResponse(await Mediator.Send(new GetSaldoRequest(idContaCorrente)));
    }
}
