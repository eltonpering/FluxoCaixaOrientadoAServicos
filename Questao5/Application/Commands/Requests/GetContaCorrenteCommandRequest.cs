using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Requests
{
    public class GetContaCorrenteCommandRequest : IRequest<IEnumerable<ContaCorrente>>
    {   
    }
}
