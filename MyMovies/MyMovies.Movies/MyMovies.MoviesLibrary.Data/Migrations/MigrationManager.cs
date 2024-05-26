using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyMovies.MoviesLibrary.Data.Migrations;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host, string databaseName)
    {
        using var scope = host.Services.CreateScope();

        var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
        var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        try
        {
            databaseService.CreateDatabase(databaseName);
            migrationService.ListMigrations();
            migrationService.MigrateUp();
        }
        catch
        {
            // Log error...
            throw;
        }
        return host;
    }
}
