
using Dapper;
using Microsoft.Extensions.Configuration;
using MyMovies.MoviesLibrary.Data.Data;

namespace MyMovies.MoviesLibrary.Data.Migrations;

public class Database
{
    private readonly string _connectionString;
    private readonly DapperContext _context;
    public Database(DapperContext context, IConfiguration configuration)
    {
        _context = context;
        this._connectionString = configuration.GetConnectionString("MasterConnectionString")!;
    }
    public void CreateDatabase(string dbName)
    {
        var query = "SELECT * FROM sys.databases WHERE name = @name";
        var parameters = new DynamicParameters();
        parameters.Add("name", dbName);
        using var connection = _context.CreateConnection(this._connectionString);
        var records = connection.Query(query, parameters);
        if (!records.Any())
        {
            connection.Execute($"CREATE DATABASE {dbName}");
        }
    }
}
