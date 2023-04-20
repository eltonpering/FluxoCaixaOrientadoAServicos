using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories;

public interface IIdempotenciaRepository
{
    Task AddIdempotencyKeyAsync(Idempotencia idempotencia);
    Task<Idempotencia> GetIdempotencyKeyByRequestIdAsync(string chaveidempotencia);
}