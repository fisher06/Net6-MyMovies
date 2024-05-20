using FluentMigrator;

namespace MyMovies.MoviesLibrary.Data.Migrations;

/// <summary>
/// run by vs
/// (localdb)\MSSQLLocalDB to connect the MSSQLLocalDB
/// run by cmd
///   SqlLocalDB.exe info MSSQLLocalDB
///   SqlLocalDB.exe create MSSQLLocalDB
///   SqlLocalDB.exe start MSSQLLocalDB
///   
///  dotnet fm migrate -p SqlServer2014 -c "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbMyMovies20240515;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=False" -a .\MyMovies.MoviesLibrary.Data.dll up 
///  D:\GIT\Formation\MyMovies\MyMovies.Movies\MyMovies.MoviesLibrary.Data\bin\Debug\net8.0
///  dotnet fm migrate -p SqlServer2014 -c "chaine de connection"
/// </summary>
[Migration(2024051511)]
public class CreateDatabase_2024051511 : Migration
{
    public override void Down()
    {
        Delete.Table("Producer");
        Delete.Table("Character");
        Delete.Table("Movie");
        Delete.Table("Actor");
    }

    public override void Up()
    {
        Create.Table("Actor")
          .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
          .WithColumn("FirstName").AsString(4000).NotNullable()
          .WithColumn("LastName").AsString(4000).NotNullable()
          .WithColumn("BirthDate").AsDateTime2().NotNullable();

        Create.Table("Movie")
          .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
          .WithColumn("Title").AsString(int.MaxValue).NotNullable()
          .WithColumn("ReleaseDate").AsInt32().NotNullable()
          .WithColumn("Runtime").AsTime().Nullable()
          .WithColumn("Resume").AsString(int.MaxValue).NotNullable();

        Create.Table("Character")
          .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
          .WithColumn("Name").AsString(int.MaxValue).NotNullable()
          .WithColumn("ActorId").AsInt32().NotNullable().ForeignKey("Actor", "Id")
          .WithColumn("MovieId").AsInt32().NotNullable().ForeignKey("Movie", "Id");

        Create.Table("Producer")
          .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
          .WithColumn("FirstName").AsString(4000).NotNullable()
          .WithColumn("LastName").AsString(4000).NotNullable()
          .WithColumn("MovieId").AsInt32().NotNullable().ForeignKey("Movie", "Id");

    }
}
