using System.Data;
using Dapper;
using MyMovies.MoviesLibrary.Data.Data;
using MyMovies.MoviesLibrary.Domain;

namespace MyMovies.MoviesLibrary.Data.Repository.impl
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DapperContext _context;
        public MovieRepository(DapperContext context)
        {
            this._context = context;
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
            movie.Id = await connection.QuerySingleAsync<int>("INSERT INTO Movie " +
              "(Title, ReleaseDate, Runtime, [Resume]) " +
              "VALUES (@Title, @ReleaseDate, @Runtime, @Resume);" +
              "SELECT CAST(SCOPE_IDENTITY() as int);", parameters);
            return movie;
        }


        public async Task<int> Delete(Movie? movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }
            using var connection = _context.CreateConnection();

            // Suppression du casting mais pas des acteurs
            await connection.ExecuteAsync("DELETE FROM Character " +
                "WHERE MovieId = @Id", new { movie.Id });

            return await connection.ExecuteAsync("DELETE FROM Movie " +
              "WHERE Id = @Id", new { movie.Id });
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Movie>("SELECT Id, Title, ReleaseDate, Runtime, [Resume] FROM Movie");
        }

        public async Task<Movie?> GetById(int? id)
        {
            using var connection = _context.CreateConnection();
            //     var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            //"SELECT Id, Title, ReleaseDate, Runtime, [Resume] FROM Movie WHERE Id = @Id",
            //new { id });
            var characterMap = new CustomPropertyTypeMap(typeof(Character), (type, columnName)
                => columnName == "CharacterId" ? type.GetProperty("Id")! : type.GetProperty(columnName)!);
            SqlMapper.SetTypeMap(typeof(Character), characterMap);

            var actorMap = new CustomPropertyTypeMap(typeof(Actor), (type, columnName)
                => columnName == "ActorId" ? type.GetProperty("Id")! : type.GetProperty(columnName)!);
            SqlMapper.SetTypeMap(typeof(Actor), actorMap);

            Movie? firstMovie = null;
            var movie = connection.QueryAsync<Movie, Character, Actor, Movie>("SELECT " +
                "Movie.Id,Movie.Title,Movie.ReleaseDate,Movie.Runtime,Movie.[Resume]," +
                "Character.Id AS CharacterId, Character.Name, " +
                "[Actor].Id AS ActorId,[Actor].FirstName,[Actor].LastName,[Actor].BirthDate " +
                "FROM Movie " +
                "INNER JOIN [Character] ON [Character].MovieId = Movie.Id " +
                "INNER JOIN [Actor] ON [Actor].Id = [Character].ActorId " +
                " WHERE Movie.Id = @Id", (movie, character, actor) =>
                {
                    if (firstMovie == null)
                    {
                        firstMovie = movie;
                        firstMovie.Cast = new List<Character>();
                    }
                    if (character != null)
                    {
                        character.Actor = actor;
                        firstMovie.Cast!.Add(character);
                    }
                    return firstMovie;
                },
                new { id },
                splitOn: "CharacterId, ActorId").Result.FirstOrDefault();
            return movie;
        }


        public async Task<Movie> Update(Movie? movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }
            DynamicParameters parameters = new();
            parameters.Add("Id", movie.Id, DbType.Int32);
            parameters.Add("Title", movie.Title, DbType.String);
            parameters.Add("ReleaseDate", movie.ReleaseDate, DbType.Int32);
            parameters.Add("Runtime", movie.Runtime, DbType.Time);
            parameters.Add("Resume", movie.Resume, DbType.String);
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("UPDATE Movie SET " +
            "Title = @Title," +
            "ReleaseDate = @ReleaseDate," +
            "Runtime = @Runtime," +
            "[Resume] = @Resume " +
            "WHERE Id = @Id", parameters);
            return movie;
        }

    }
}
