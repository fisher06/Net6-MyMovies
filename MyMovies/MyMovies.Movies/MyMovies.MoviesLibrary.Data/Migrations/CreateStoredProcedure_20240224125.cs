using FluentMigrator;
using System.Reflection;

namespace MyMovies.MoviesLibrary.Data.Migrations;

[Migration(20240224125)]
public class CreateStoredProcedure_20240224125 : Migration
{
    public override void Up()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames().Where(str => str.EndsWith("CreateStoredProcedure.sql"));
        foreach (string resourceName in resourceNames)
        {
            using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
            using StreamReader reader = new StreamReader(stream);

            string sql = reader.ReadToEnd();
            Execute.Sql(sql);

        }
    }

    public override void Down()
    {
        Execute.Sql("DROP PROCEDURE GetActorById;");
        Execute.Sql("DROP PROCEDURE GetAllActor;");
    }
}

