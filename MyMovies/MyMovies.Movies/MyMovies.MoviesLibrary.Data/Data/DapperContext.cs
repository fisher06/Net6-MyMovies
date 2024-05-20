using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyMovies.MoviesLibrary.Data.Data
{
    public class DapperContext
    {
        private readonly DbProviderFactory dbProviderFactory;
        private readonly IConfiguration configuration;
        private readonly string? connexionString;


        public DapperContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connexionString = this.configuration.GetConnectionString("SqlConnectionString");
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
        }

        public IDbConnection CreateConnection() => CreateConnection(this.connexionString);

        public IDbConnection CreateConnection(string? connectionString)
        {
            var connection = this.dbProviderFactory.CreateConnection();
            connection!.ConnectionString = connectionString;
            return connection;
        }
    }
}
