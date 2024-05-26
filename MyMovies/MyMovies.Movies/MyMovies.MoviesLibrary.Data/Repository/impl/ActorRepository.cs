using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;
using MyMovies.MoviesLibrary.Data.Data;
using MyMovies.MoviesLibrary.Domain;
using System.Data;
using System.Threading;
using Z.Dapper.Plus;

namespace MyMovies.MoviesLibrary.Data.Repository.Impl;

public class ActorRepository : IActorRepository
{
    private readonly DapperContext _context;

    public ActorRepository(DapperContext context)
    {
        this._context = context;
    }

    public async Task<Actor> Create(Actor? actor, IDbTransaction? transaction = null)
    {
        if (actor == null)
        {
            throw new ArgumentNullException(nameof(actor));
        }
        DynamicParameters parameters = new();
        parameters.Add("FirstName", actor.FirstName, DbType.String);
        parameters.Add("LastName", actor.LastName, DbType.String);
        parameters.Add("BirthDate", actor.BirthDate, DbType.DateTime2);

        var sql = "INSERT INTO Actor " +
            "(FirstName, LastName, BirthDate) " +
            "VALUES (@FirstName, @LastName, @BirthDate);" +
            "SELECT CAST(SCOPE_IDENTITY() as int);";
        if (transaction == null)
        {
            using var connection = _context.CreateConnection();
            actor.ID = await connection.QuerySingleAsync<int>(sql, parameters);
        }
        else
        {
            // Attention ! transaction et connection ouvertes
            actor.ID = await transaction.QuerySingleAsync<int>(sql, parameters);
        }

        return actor;
    }

    public async Task<int> Delete(Actor? actor, IDbTransaction? transaction = null)
    {
        if (actor == null)
        {
            throw new ArgumentNullException(nameof(actor));
        }

        var sql = "DELETE FROM Actor WHERE ID = @ID";
        if (transaction == null)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(sql, new { actor.ID });
        }
        else
        {
            // Attention ! transaction et connection ouvertes
            return await transaction.ExecuteAsync(sql, new { actor.ID });
        }
    }

    public async Task<IEnumerable<Actor>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var actors = await connection.QueryAsync<Actor>("GetAllActor", commandType: CommandType.StoredProcedure);
        return actors;
    }

    public async Task<Actor?> GetById(int? id)
    {
        using var connection = _context.CreateConnection();
        var actor = await connection.QuerySingleOrDefaultAsync<Actor>("GetActorById", new { id }, commandType: CommandType.StoredProcedure);
        return actor;
    }

    public async Task<Actor> Update(Actor? actor, IDbTransaction? transaction = null)
    {
        if (actor == null)
        {
            throw new ArgumentNullException(nameof(actor));
        }
        DynamicParameters parameters = new();
        parameters.Add("ID", actor.ID, DbType.Int32);
        parameters.Add("FirstName", actor.FirstName, DbType.String);
        parameters.Add("LastName", actor.LastName, DbType.String);
        parameters.Add("BirthDate", actor.BirthDate, DbType.DateTime2);

        var sql = "UPDATE Actor SET " +
            "FirstName = @FirstName," +
            "LastName = @LastName," +
            "BirthDate = @BirthDate " +
            "WHERE ID = @ID";

        if (transaction == null)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, parameters);
        }
        else
        {
            // Attention ! transaction et connection ouvertes
            await transaction.ExecuteAsync(sql, parameters);
        }
        return actor;
    }

    //public void Create(IEnumerable<Actor> actors)
    //{
    //    using var connection = _context.CreateConnection();

    //    // Définit la clé primaire
    //    DapperPlusManager.Entity<Actor>().Identity(x => x.ID, true);

    //    // Insertion en masse
    //    connection.BulkInsert(actors);

    //}

    //public async Task Update(IEnumerable<Actor> actors)
    //{
    //    using var connection = _context.CreateConnection();

    //    // Définit la clé primaire
    //    DapperPlusManager.Entity<Actor>().Identity(x => x.ID, true);

    //    // Modification en masse asynchrone
    //    await connection.BulkActionAsync(x => x.BulkUpdate(actors));
    //}
}
