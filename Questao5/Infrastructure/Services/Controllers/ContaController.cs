using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Services.Models;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[Route("[controller]")]
public class ContaController : PublicControllerBase
{
    public ContaController(IServiceContext serviceContext) : base(serviceContext)
    {
       
    }

    /// <summary>
    /// Lista todas as contas correntes
    /// </summary>
    /// <returns>Retorna todas as contas e status 200 ok</returns>
    /// <response code="200">Contas Correntes Listadas</response>
    [HttpGet("listacontacorrente")]
    [ProducesResponseType(typeof(IEnumerable<ContaCorrente>), 200)]
    [ProducesResponseType(typeof(BadRequestModel), 400)]
    public async Task<IActionResult> GetAll()
    {   
        return GetResponse(await Mediator.Send(new GetContaCorrenteCommandRequest()));
    }
}
