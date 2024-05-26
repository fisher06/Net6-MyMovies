using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;
using MyMovies.MoviesLibrary.Data.Data;
using MyMovies.MoviesLibrary.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Transactions;

namespace MyMovies.MoviesLibrary.Data.Repository.Impl;

public class MovieRepository : IMovieRepository
{
    private readonly DapperContext _context;
    private readonly IActorRepository _actorRepository;

    public MovieRepository(DapperContext context, IActorRepository actorRepository)
    {
        this._context = context;
        this._actorRepository = actorRepository;
    }

    public async Task<IEnumerable<Movie>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var movies = await connection.QueryAsync<Movie>("SELECT ID, Title, ReleaseDate, Runtime, [Resume] FROM Movie");
        return movies;
    }

    public async Task<Movie?> GetById(int? id)
    {
        using var connection = _context.CreateConnection();
        // var movie = await connection.QuerySingleOrDefaultAsync<Movie>("SELECT ID, Title, ReleaseDate, Runtime, [Resume] FROM Movie WHERE ID = @Id", new { id });

        //var movie = connection.QueryAsync<Movie>("SELECT Movie.ID,Movie.Title,Movie.ReleaseDate,Movie.Runtime,Movie.[Resume]," +
        //    "Character.MovieID, Character.ID AS CharacterID, Character.Name, " +
        //    "[Actor].ID AS ActorID,[Actor].FirstName,[Actor].LastName,[Actor].BirthDate " +
        //    "FROM Movie " +
        //    "INNER JOIN [Character] ON [Character].MovieID = Movie.ID " +
        //    "INNER JOIN [Actor] ON [Actor].ID = [Character].ActorID " +
        //    " WHERE Movie.ID = @Id", new { id }).Result.FirstOrDefault();

        //// Map Character
        //var characterMap = new CustomPropertyTypeMap(typeof(Character), (type, columnName) => columnName == "CharacterID" ? type.GetProperty("ID")! : type.GetProperty(columnName)!);
        //SqlMapper.SetTypeMap(typeof(Character), characterMap);
        //// Map Actor
        //var actorMap = new CustomPropertyTypeMap(typeof(Actor), (type, columnName) => columnName == "ActorID" ? type.GetProperty("ID")! : type.GetProperty(columnName)!);
        //SqlMapper.SetTypeMap(typeof(Actor), actorMap);

        //var movieDictionary = new Dictionary<int, Movie>();
        //var movie = connection.QueryAsync<Movie, Character, Actor, Movie>("SELECT " +
        //    "Movie.ID,Movie.Title,Movie.ReleaseDate,Movie.Runtime,Movie.[Resume]," +
        //    "Character.MovieID, Character.ID AS CharacterID, Character.Name, " +
        //    "[Actor].ID AS ActorID,[Actor].FirstName,[Actor].LastName,[Actor].BirthDate " +
        //    "FROM Movie " +
        //    "INNER JOIN [Character] ON [Character].MovieID = Movie.ID " +
        //    "INNER JOIN [Actor] ON [Actor].ID = [Character].ActorID " +
        //    " WHERE Movie.ID = @Id",
        //    (movie, character, actor) =>
        //    {
        //        Movie? movieEntry;
        //        if (!movieDictionary.TryGetValue(movie.ID, out movieEntry))
        //        {
        //            movieEntry = movie;
        //            movieEntry.Cast = new List<Character>();
        //            movieDictionary.Add(movieEntry.ID, movieEntry);
        //        }
        //        character.Actor = actor;
        //        movieEntry.Cast!.Add(character);
        //        return movieEntry;
        //    },
        //    new { id },
        //    splitOn: "CharacterID, ActorID").Result.Distinct().FirstOrDefault();

        // Map avec producer
        var characterMap = new CustomPropertyTypeMap(typeof(Character), (type, columnName) => columnName == "CharacterID" ? type.GetProperty("ID")! : type.GetProperty(columnName)!);
        SqlMapper.SetTypeMap(typeof(Character), characterMap);


        var actorMap = new CustomPropertyTypeMap(typeof(Actor), (type, columnName) => columnName == "ActorID" ? type.GetProperty("ID")! : type.GetProperty(columnName)!);
        SqlMapper.SetTypeMap(typeof(Actor), actorMap);

        //Dictionary<string, PropertyInfo?> producerMapping = new()
        //{
        //    { "ProcuderID", typeof(Producer).GetProperty("ID") },
        //    { "ProducerFirstName", typeof(Producer).GetProperty("FirstName") },
        //    { "ProducerLastName", typeof(Producer).GetProperty("LastName") },
        //};
        //var producerMap = new CustomPropertyTypeMap(typeof(Producer), (type, columnName) =>
        //    producerMapping.TryGetValue(columnName, out var producerPropertyInfo) ? producerPropertyInfo! : type.GetProperty(columnName)!);
        //SqlMapper.SetTypeMap(typeof(Producer), producerMap);
        Dapper.SqlMapper.SetTypeMap(typeof(Producer),
            new CustomPropertyTypeMap(typeof(Producer), (type, columnName) =>
            type.GetProperties().FirstOrDefault(prop =>
                 prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr!.Name == columnName))!));

        var movieDictionary = new Dictionary<int, Movie>();
        var movie = connection.QueryAsync<Movie, Producer, Character, Actor, Movie>("SELECT Movie.ID,Movie.Title,Movie.ReleaseDate,Movie.Runtime,Movie.[Resume]," +
                    "[Producer].ID as ProcuderID, [Producer].FirstName as ProducerFirstName , [Producer].LastName as ProducerLastName," +
                 "[Character].ID AS CharacterID, [Character].[Name]," +
                 "[Actor].ID AS ActorID,[Actor].FirstName,[Actor].LastName,[Actor].BirthDate " +
                 "FROM Movie " +
                 "LEFT JOIN Producer ON Producer.MovieID = Movie.ID " +
                 "LEFT JOIN [Character] ON [Character].MovieID = Movie.ID " +
                 "LEFT JOIN [Actor] ON [Actor].ID = [Character].ActorID " +
                    "WHERE Movie.ID = @Id",
            (movie, producer, character, actor) =>
            {
                Movie? movieEntry;
                if (!movieDictionary.TryGetValue(movie.ID, out movieEntry))
                {
                    movieEntry = movie;
                    movieEntry.Cast = new List<Character>();
                    movieEntry.Producers = new List<Producer>();
                    movieDictionary.Add(movieEntry.ID, movieEntry);
                }
                if (producer != null && movieEntry.Producers!.FirstOrDefault(p => p.ID == producer.ID) == null)
                {
                    movieEntry.Producers!.Add(producer);
                }
                if (character != null && movieEntry.Cast!.FirstOrDefault(p => p.ID == character.ID) == null)
                {
                    character.Actor = actor;
                    movieEntry.Cast!.Add(character);
                }
                return movieEntry;
            },
            new { id },
            splitOn: "ProcuderID, CharacterID, ActorID").Result.Distinct().FirstOrDefault();

        return movie;
    }

    public async Task<Movie> Create(Movie? movie)
    {
        if (movie == null)
        {
            throw new ArgumentNullException(nameof(movie));
        }
        DynamicParameters parameters = new();
        parameters.Add("Title", movie.Title, DbType.String);
        parameters.Add("ReleaseDate", movie.ReleaseDate, DbType.Int32);
        parameters.Add("Runtime", movie.Runtime, DbType.Time);
        parameters.Add("Resume", movie.Resume, DbType.String);

        using var connection = _context.CreateConnection();
        connection.Open();
        using IDbTransaction transaction = connection.BeginTransaction();
        try
        {
            // Tratiement du film sans dépendance
            movie.ID = await transaction.QuerySingleAsync<int>("INSERT INTO Movie " +
           "(Title, ReleaseDate, Runtime, [Resume]) " +
           "VALUES (@Title, @ReleaseDate, @Runtime, @Resume);" +
           "SELECT CAST(SCOPE_IDENTITY() as int);", parameters);

            // Traitement du casting
            if (movie.Cast != null)
            {
                foreach (var character in movie.Cast)
                {
                    if (character.Actor != null)
                    {
                        Actor? actor = null;
                        if (character.Actor.ID > 0 && await _actorRepository.GetById(character.Actor.ID) != null)
                        {
                            actor = await _actorRepository.Update(character.Actor, transaction);
                        }
                        else
                        {
                            actor = await _actorRepository.Create(character.Actor, transaction);
                        }
                        character.Actor = actor; // mise a jour l'ID Actor si création d'un nouveau               

                        // Traitement des Charaters
                        DynamicParameters charactersParameters = new();
                        charactersParameters.Add("Name", character.Name, DbType.String);
                        charactersParameters.Add("ActorID", character.Actor.ID, DbType.Int32);
                        charactersParameters.Add("MovieID", movie.ID, DbType.Int32);

                        character.ID = await transaction.QuerySingleAsync<int>("INSERT INTO Character " +
                                "([Name], ActorID, MovieID) " +
                                "VALUES (@Name, @ActorID, @MovieID);" +
                                "SELECT CAST(SCOPE_IDENTITY() as int);", charactersParameters);
                    }
                }
            }

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
        }
        return movie;
    }

    public async Task<Movie> Update(Movie? movie)
    {
        if (movie == null)
        {
            throw new ArgumentNullException(nameof(movie));
        }
        DynamicParameters parameters = new();
        parameters.Add("ID", movie.ID, DbType.Int32);
        parameters.Add("Title", movie.Title, DbType.String);
        parameters.Add("ReleaseDate", movie.ReleaseDate, DbType.Int32);
        parameters.Add("Runtime", movie.Runtime, DbType.Time);
        parameters.Add("Resume", movie.Resume, DbType.String);

        using var connection = _context.CreateConnection();
        connection.Open();
        using IDbTransaction transaction = connection.BeginTransaction();
        try
        {
            // Tratiement du film sans dépendance
            await transaction.ExecuteAsync("UPDATE Movie SET " +
            "Title = @Title," +
            "ReleaseDate = @ReleaseDate," +
            "Runtime = @Runtime," +
            "[Resume] = @Resume " +
            "WHERE ID = @ID", parameters);
            
            // Supprime le casting et Supprime les acteurs orphelins
            await transaction.ExecuteAsync("DELETE FROM Character WHERE MovieID = @MovieID", new { MovieID = movie.ID });            
            await transaction.ExecuteAsync("DELETE FROM Actor WHERE ID NOT IN (SELECT ActorID FROM Character)");

            // Traitement du nouveau casting
            if (movie.Cast != null)
            {
                foreach (var character in movie.Cast)
                {
                    if (character.Actor != null)
                    {
                        Actor? actor = null;
                        if (character.Actor.ID > 0 && await _actorRepository.GetById(character.Actor.ID) != null)
                        {
                            actor = await _actorRepository.Update(character.Actor, transaction);
                        }
                        else
                        {
                            actor = await _actorRepository.Create(character.Actor, transaction);
                        }
                        character.Actor = actor; // mise a jour l'ID Actor si création d'un nouveau               

                        // Traitement des Charaters
                        DynamicParameters charactersParameters = new();
                        charactersParameters.Add("Name", character.Name, DbType.String);
                        charactersParameters.Add("ActorID", character.Actor.ID, DbType.Int32);
                        charactersParameters.Add("MovieID", movie.ID, DbType.Int32);

                        character.ID = await transaction.QuerySingleAsync<int>("INSERT INTO Character " +
                                "([Name], ActorID, MovieID) " +
                                "VALUES (@Name, @ActorID, @MovieID);" +
                                "SELECT CAST(SCOPE_IDENTITY() as int);", charactersParameters);
                    }
                }
            }

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
        }
        return movie;
    }

    public async Task<int> Delete(Movie? movie)
    {
        var result = -1;
        if (movie == null)
        {
            throw new ArgumentNullException(nameof(movie));
        }
        using var connection = _context.CreateConnection();
        connection.Open();
        using IDbTransaction transaction = connection.BeginTransaction();
        try
        {
            // Suppression du casting mais pas des acteurs
            await transaction.ExecuteAsync("DELETE FROM Character " +
            "WHERE MovieID = @ID", new { movie.ID });

            result = await transaction.ExecuteAsync("DELETE FROM Movie " +
            "WHERE ID = @ID", new { movie.ID });

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
        }
        return result;
    }

}

