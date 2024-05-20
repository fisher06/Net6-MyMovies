using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyMovies.MoviesLibrary.Data.Migrations
{
    public static class MigrationManager
    {
        public static IHost MigrationDatabase(this IHost host, string databaseName)
        {
            using var scope = host.Services.CreateScope();

            var migrationService = scope.ServiceProvider.GetService<IMigrationRunner>();
            var databaseService = scope.ServiceProvider.GetService<Database>();

            databaseService!.CreateDatabase(databaseName);
            migrationService!.ListMigrations();
            migrationService!.MigrateUp();

            return host;
        }
    }
}
