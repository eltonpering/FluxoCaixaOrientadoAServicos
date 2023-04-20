using Questao5.Application.ViewModels;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories;

public interface IMovimentoRepository
{
    Task<IList<MovimentoViewModel>> GetContaCorrenteByNumeroAsync(int numeroContaCorrente);
    
    //Serviço: Movimentação de uma conta corrente
    Task<string> AddMovimentoAsync(Movimento movimento);

    //Serviço: Saldo da conta corrente
    Task<SaldoContaCorrenteViewModel> GetSaldoContaCorrenteByIdAsync(string contacorrenteId);
}
