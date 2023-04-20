using Microsoft.EntityFrameworkCore;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;
using Microsoft.Data.Sqlite;
using Dapper;

namespace Questao5.Infrastructure.Repositories;

public class IdempotenciaRepository : IIdempotenciaRepository
{
    private readonly string connectionString;

    public IdempotenciaRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task AddIdempotencyKeyAsync(Idempotencia idempotencia)
    {
        using var connection = new SqliteConnection(connectionString);

        const string insertSql = @"
                INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                VALUES (@chaveidempotencia, @requisicao, @resultado);
            ";
        await connection.ExecuteAsync(insertSql, new
        {
            chaveidempotencia = idempotencia.ChaveIdempotencia,
            requisicao = idempotencia.Requisicao,
            resultado = idempotencia.Resultado
        });
        connection.Close();
    }

    public async Task<Idempotencia> GetIdempotencyKeyByRequestIdAsync(string chaveidempotencia)
    {
        using var connection = new SqliteConnection(connectionString);

        var idempotencyKey = await connection.QuerySingleOrDefaultAsync<Idempotencia>(
            "SELECT chave_idempotencia AS chaveidempotencia, requisicao, resultado FROM idempotencia WHERE chave_idempotencia = @Chaveidempotencia",
            new { Chaveidempotencia = chaveidempotencia });
        connection.Close();

        return idempotencyKey;
    }
}