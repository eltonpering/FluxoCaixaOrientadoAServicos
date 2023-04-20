using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.ViewModels;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Infrastructure.Repositories;

public class MovimentoRepository : IMovimentoRepository
{
    private readonly string connectionString;

    public MovimentoRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<IList<MovimentoViewModel>> GetContaCorrenteByNumeroAsync(int numeroContaCorrente)
    {
        using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        const string sql = @"SELECT m.* FROM movimento m 
                                 INNER JOIN contacorrente cc ON cc.idcontacorrente = m.idcontacorrente
                                 WHERE cc.numero = @numeroContaCorrente
                                 ORDER BY m.datamovimento DESC";
        var movimentos = (await connection.QueryAsync<MovimentoViewModel>(sql, new { numeroContaCorrente })).ToList();
        connection.Close();

        return movimentos;
    }

    public async Task<string> AddMovimentoAsync(Movimento movimento)
    {
        using var connection = new SqliteConnection(connectionString);

        const string insertSql = @"
                INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                VALUES (@idMovimento, @idcontacorrente, @datamovimento, @tipomovimento, @valor);
            ";
        await connection.ExecuteAsync(insertSql, new
        {
            idMovimento = movimento.IdMovimento.ToString(),
            idcontacorrente = movimento.IdContaCorrente,
            datamovimento = movimento.DataMovimento,
            tipomovimento = movimento.TipoMovimento,
            valor = movimento.Valor
        });
        connection.Close();

        return movimento.IdMovimento.ToString();
    }

    public async Task<SaldoContaCorrenteViewModel> GetSaldoContaCorrenteByIdAsync(string contacorrenteId)
    {
        using var connection = new SqliteConnection(connectionString);

        var query = @"
                SELECT 
                    C.Numero as NumeroContaCorrente,
                    C.Nome as NomeTitular,
                    datetime('now') AS DataConsulta,
                    COALESCE(SUM(CASE M.tipomovimento WHEN 'C' THEN M.valor ELSE 0 END), 0) -
                                COALESCE(SUM(CASE M.tipomovimento WHEN 'D' THEN M.valor ELSE 0 END), 0) AS saldo
                FROM contacorrente C
                LEFT JOIN movimento M ON C.IdContaCorrente = M.IdContaCorrente
                WHERE C.IdContaCorrente = @contacorrenteId
                GROUP BY C.Numero, C.Nome"
        ;

        var saldoContaModel = await connection.QueryFirstOrDefaultAsync<SaldoContaCorrenteViewModel>(query, new { contacorrenteId });      

        return saldoContaModel;
    }
}
