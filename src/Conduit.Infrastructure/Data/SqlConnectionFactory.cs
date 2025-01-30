using System.Data;
using Conduit.Application.Abstractions.Data;
using Npgsql;

namespace Conduit.Infrastructure.Data;

public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    private readonly string _connectionString = connectionString;

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);

        connection.Open();
        
        return connection;
    }
}
