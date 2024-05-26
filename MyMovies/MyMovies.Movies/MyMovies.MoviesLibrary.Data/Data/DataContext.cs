using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data;

namespace MyMovies.MoviesLibrary.Data.Data;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private readonly DbProviderFactory _dbFactory;

    public DapperContext(IConfiguration configuration)
    {
        this._configuration = configuration;
        this._connectionString = this._configuration.GetConnectionString("SqlConnectionString")!;
        DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
        this._dbFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
    }

    public IDbConnection CreateConnection()
    {
        var connection = this._dbFactory.CreateConnection();
        connection!.ConnectionString = this._connectionString;
        return connection;
    }
    public IDbConnection CreateConnection(string connectionString)
    {
        var connection = this._dbFactory.CreateConnection();
        connection!.ConnectionString = connectionString;
        return connection;
    }
}

