using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Interfaces;

namespace Questao5.Infrastructure.Services.Controllers;


[Route("api/[controller]")]
public abstract class PublicControllerBase : ContaControllerBase
{
    protected PublicControllerBase(IServiceContext serviceContext) : base(serviceContext)
    {
    }
}
