using Microsoft.AspNetCore.Mvc;
using MediatR;
using Questao5.Application.Interfaces;
using Questao5.Infrastructure.Services.Models;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
public abstract class ContaControllerBase : ControllerBase
{
    private ISender _mediator = null!;
    private readonly IServiceContext _serviceContext;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ContaControllerBase(IServiceContext serviceContext)
    {
        _serviceContext = serviceContext;
    }

    public IActionResult GetResponse<T>(T response)
    {
        if (_serviceContext.HasError())
        {
            return BadRequest(ErrorBuilder());
        }
        else if (_serviceContext.HasEntityError())
        {
            return BadRequest(EntityErrorBuilder());
        }
        else if (response is null)
        {
            return NoContent();
        }
        else
        {
            return Ok(response);
        }
    }

    private BadRequestModel EntityErrorBuilder()
    {
        var model = new BadRequestModel
        {
            TraceId = HttpContext.TraceIdentifier,
            Instance = HttpContext.Request.Path.Value?.Replace("/api", string.Empty)
        };

        if (_serviceContext.EntityErrors.Any())
        {
            model.Errors = new List<BadRequestErrorModel>();
            foreach (var propertyErrors in _serviceContext.EntityErrors)
            {
                foreach (var error in propertyErrors.Value)
                {
                    model.Errors.Add(new BadRequestErrorModel
                    {
                        Error = error,
                        Detail = error,
                        Property = propertyErrors.Key
                    });
                }
            }
        }

        return model;
    }

    private BadRequestModel ErrorBuilder()
    {
        return new BadRequestModel
        {
            TraceId = HttpContext.TraceIdentifier,
            Instance = HttpContext.Request.Path.Value?.Replace("/api", string.Empty),
            Error = new BadRequestErrorModel
            {
                Error = _serviceContext.Errors.FirstOrDefault(),
                Detail = _serviceContext.Errors.FirstOrDefault()
            }
        };
    }
}
