using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories;

public interface IContaCorrenteRepository
{
    Task<ContaCorrente> GetByIdAsync(string contacorrenteId);
    Task<ContaCorrente> GetByNumeroAsync(int number);
    Task<IEnumerable<ContaCorrente>> GetAllAsync();
    Task UpdateAsync(ContaCorrente account);
    Task<bool> ContaCorrenteExisteByIdAsync(string contacorrenteId);
    Task<bool> ContaCorrrenteAtivaByIdAsync(string contacorrenteId);
    Task<bool> ContaCorrenteExisteByNumeroAsync(int numeroContaCorrente);    
}
