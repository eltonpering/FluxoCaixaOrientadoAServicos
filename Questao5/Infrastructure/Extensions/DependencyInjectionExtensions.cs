using MediatR;
using Questao5.Application.Commands.Handlers;
using Questao5.Application.Interfaces;
using Questao5.Application.Services.Base;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Repositories;

namespace Questao5.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddDependencies(this IServiceCollection services, string connectionString)
    {
        // Application
        services.AddMediatR(typeof(CreateMovimentoCommandHandler).Assembly);

        // Domain   

        // Infrastructure
        services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>(provider => new ContaCorrenteRepository(connectionString));
        services.AddScoped<IMovimentoRepository, MovimentoRepository>(provider => new MovimentoRepository(connectionString));
        services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>(provider => new IdempotenciaRepository(connectionString));
        services.AddScoped<IServiceContext, ServiceContext>();            
    }
}
