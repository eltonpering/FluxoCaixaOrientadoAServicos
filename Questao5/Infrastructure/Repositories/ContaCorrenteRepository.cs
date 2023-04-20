using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly string connectionString;

    public ContaCorrenteRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<ContaCorrente> GetByIdAsync(string id)
    {
        using var connection = new SqliteConnection(connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<ContaCorrente>(
            @"SELECT idcontacorrente AS IdContaCorrente, numero AS Numero, nome AS Nome, ativo AS Ativo
                FROM contacorrente
                WHERE idcontacorrente = @id",
            new { id }
        );
        connection.Close();
        return result;
    }

    public async Task<ContaCorrente> GetByNumeroAsync(int number)
    {
        using var connection = new SqliteConnection(connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<ContaCorrente>(
            @"SELECT idcontacorrente AS IdContaCorrente, numero AS Numero, nome AS Nome, ativo AS Ativo
                FROM contacorrente
                WHERE numero = @number",
            new { number }
        );
        connection.Close();
        return result;
    }

    public async Task<IEnumerable<ContaCorrente>> GetAllAsync()
    {
        using var connection = new SqliteConnection(connectionString);
        var result = await connection.QueryAsync<ContaCorrente>(
            @"SELECT idcontacorrente AS IdContaCorrente, numero AS Numero, nome AS Nome, ativo AS Ativo
                FROM contacorrente"
        );
        connection.Close();
        return result;
    }

    public async Task UpdateAsync(ContaCorrente contacorrente)
    {
        using var connection = new SqliteConnection(connectionString);
        await connection.ExecuteAsync(
            @"UPDATE contacorrente
                SET nome = @nome,
                    ativo = @ativo
                WHERE idcontacorrente = @contacorrenteId",
            new { contacorrenteId = contacorrente.IdContaCorrente, nome = contacorrente.Nome, ativo = contacorrente.Ativo }
        );
        connection.Close();
    }

    public async Task<bool> ContaCorrenteExisteByIdAsync(string contacorrenteId)
    {
        using var connection = new SqliteConnection(connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<int>(
            @"SELECT COUNT(*) FROM contacorrente WHERE idcontacorrente = @contacorrenteId",
            new { contacorrenteId }
        );
        connection.Close();
        return result > 0;
    }

    public async Task<bool> ContaCorrrenteAtivaByIdAsync(string contacorrenteId)
    {
        using var connection = new SqliteConnection(connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<int>(
            @"SELECT COUNT(*) FROM contacorrente WHERE idcontacorrente = @contacorrenteId AND ativo = 1",
            new { contacorrenteId }
        );
        connection.Close();
        return result > 0;
    }

    public async Task<bool> ContaCorrenteExisteByNumeroAsync(int numeroContaCorrente)
    {
        using var connection = new SqliteConnection(connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<int>(
            @"SELECT COUNT(*) FROM contacorrente WHERE numero = @numeroContaCorrente",
            new { numeroContaCorrente }
        );
        connection.Close();
        return result > 0;
    }
}