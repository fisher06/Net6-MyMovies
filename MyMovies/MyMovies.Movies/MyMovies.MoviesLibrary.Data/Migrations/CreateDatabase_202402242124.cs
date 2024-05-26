using FluentMigrator;

namespace MyMovies.MoviesLibrary.Data.Migrations;

[Migration(202402242124)]
public class CreateDatabase_202402242124 : Migration
{
    public override void Up()
    {
        Create.Table("Actor")
            .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("FirstName").AsString(4000).NotNullable()
            .WithColumn("LastName").AsString(4000).NotNullable()
            .WithColumn("BirthDate").AsDateTime2().NotNullable();

        Create.Table("Movie")
            .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Title").AsString(int.MaxValue).NotNullable()
            .WithColumn("ReleaseDate").AsInt32().NotNullable()
            .WithColumn("Runtime").AsTime().Nullable()
            .WithColumn("Resume").AsString(int.MaxValue).NotNullable();

        Create.Table("Character")
            .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(int.MaxValue).NotNullable()
            .WithColumn("ActorID").AsInt32().NotNullable().ForeignKey("Actor", "ID")
            .WithColumn("MovieID").AsInt32().NotNullable().ForeignKey("Movie", "ID");

        Create.Table("Producer")
           .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
           .WithColumn("FirstName").AsString(4000).NotNullable()
           .WithColumn("LastName").AsString(4000).NotNullable()
           .WithColumn("MovieID").AsInt32().NotNullable().ForeignKey("Movie", "ID");
    }

    public override void Down()
    {
        Delete.Table("Character");
        Delete.Table("Producer");
        Delete.Table("Actor");
        Delete.Table("Movie");
    }
}
