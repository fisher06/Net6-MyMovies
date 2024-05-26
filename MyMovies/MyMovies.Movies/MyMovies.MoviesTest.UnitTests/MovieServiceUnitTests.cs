using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyMovies.MoviesLibrary.Business.Model;
using MyMovies.MoviesLibrary.Business.Services;
using MyMovies.MoviesLibrary.Business.Services.Impl;
using MyMovies.MoviesLibrary.Data.Repository;
using MyMovies.MoviesLibrary.Domain;
using System.Diagnostics.CodeAnalysis;

namespace MyMovies.MoviesTest.UnitTests;

[TestClass]
public class MovieServiceUnitTests
{
    // Permet de comparer deux collections de MovieDTO
    public class MovieComparer : IEqualityComparer<MovieDTO>
    {
        public bool Equals(MovieDTO? m1, MovieDTO? m2) => m1!.ID == m2!.ID && m1.Title == m2.Title && m1.ReleaseDate == m2.ReleaseDate;
        public int GetHashCode([DisallowNull] MovieDTO obj) => obj.ID.GetHashCode();
    }

    private ServiceCollection? serviceCollection;
    private IEnumerable<MovieDTO>? data = new List<MovieDTO>
        {
            new MovieDTO() { ID=1, Title="Exemple de Film 1", ReleaseDate=2023 },
            new MovieDTO() { ID=2, Title="Exemple de Film 2", ReleaseDate=2024 },
            new MovieDTO() { ID=3, Title="Exemple de Film 3", ReleaseDate=2024 }
        };

    [TestInitialize]
    public void Initialize()
    {
        serviceCollection = new ServiceCollection();
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddTransient<IMovieService, MovieService>();
    }

    [TestMethod]
    public void GetAll_TestMethod()
    {
        // Arrange
        IEnumerable<MovieDTO>? expectedMovies = data;

        serviceCollection!.AddTransient<IMovieRepository>(provider =>
        {
            var movies = provider.GetService<IMapper>()!.Map<IEnumerable<Movie>>(expectedMovies);
            var mock = new Mock<IMovieRepository>();
            mock.Setup(repository => repository.GetAll()).Returns(Task.FromResult(movies)!);
            return mock.Object;
        });
        var serviceProvider = serviceCollection!.BuildServiceProvider();

        IMovieService service = serviceProvider.GetService<IMovieService>()!;

        // Act
        var actualMovies = service.GetAll().Result;

        // Assert
        Assert.IsTrue(expectedMovies!.SequenceEqual(actualMovies, new MovieComparer()));
    }
    [TestMethod]
    public void GetById_TestMethod()
    {
        // Arrange
        MovieDTO? expectedMovie = data!.FirstOrDefault(m => m.ID == 1);

        serviceCollection!.AddTransient<IMovieRepository>(provider =>
        {
            Movie? movie = provider.GetService<IMapper>()!.Map<Movie>(expectedMovie);
            var mock = new Mock<IMovieRepository>();
            mock.Setup(repository => repository.GetById(It.IsAny<int?>())).Returns((int id) => Task.FromResult(movie)!);
            return mock.Object;
        });
        var serviceProvider = serviceCollection!.BuildServiceProvider();

        IMovieService service = serviceProvider.GetService<IMovieService>()!;

        // Act
        var actualMovie = service.GetById(1).Result;

        // Assert
        Assert.AreEqual(expectedMovie, actualMovie, new MovieComparer());
    }

    [TestMethod]
    public void Create_TestMethod()
    {
        // Arrange
        MovieDTO expectedMovie = new MovieDTO { ID = 4, Title = "new movie", ReleaseDate = 2024, Runtime = TimeSpan.Parse("1:01:00") };

        serviceCollection!.AddTransient<IMovieRepository>(provider =>
        {
            var mock = new Mock<IMovieRepository>();
            mock.Setup(repository => repository.Create(It.IsAny<Movie>())).Returns((Movie m) =>
            {
                m.ID = 4;
                return Task.FromResult(m)!;
            });
            return mock.Object;
        });
        var serviceProvider = serviceCollection!.BuildServiceProvider();

        IMovieService service = serviceProvider.GetService<IMovieService>()!;

        // Act
        var actualMovie = service.Create(new MovieDTO { Title = "new movie", ReleaseDate = 2024, Runtime = TimeSpan.Parse("1:01:00") }).Result;

        // Assert
        Assert.AreEqual(expectedMovie, actualMovie, new MovieComparer());
    }

    [TestMethod]
    public void Update_TestMethod()
    {
        // Arrange
        MovieDTO expectedMovie = new MovieDTO { ID = 1, Title = "updated movie", ReleaseDate = 2024, Runtime = TimeSpan.Parse("1:01:00") };

        serviceCollection!.AddTransient<IMovieRepository>(provider =>
        {
            var mock = new Mock<IMovieRepository>();
            mock.Setup(repository => repository.Update(It.IsAny<Movie>())).Returns((Movie m) =>
            {
                return Task.FromResult(m)!;
            });
            return mock.Object;
        });
        var serviceProvider = serviceCollection!.BuildServiceProvider();

        IMovieService service = serviceProvider.GetService<IMovieService>()!;

        // Act
        var actualMovie = service.Update(new MovieDTO { ID = 1, Title = "updated movie", ReleaseDate = 2024, Runtime = TimeSpan.Parse("1:01:00") }).Result;

        // Assert
        Assert.AreEqual(expectedMovie, actualMovie, new MovieComparer());
    }

    [TestMethod]
    public void Delete_TestMethod()
    {
        // Arrange
        int expectedvalue = 1;
        serviceCollection!.AddTransient<IMovieRepository>(provider =>
        {
            var mock = new Mock<IMovieRepository>();
            mock.Setup(repository => repository.Delete(It.IsAny<Movie>())).Returns((Movie m) =>
            {
                return Task.FromResult(expectedvalue);
            });
            return mock.Object;
        });
        var serviceProvider = serviceCollection!.BuildServiceProvider();

        IMovieService service = serviceProvider.GetService<IMovieService>()!;

        // Act
        var actualValue = service.Delete(new MovieDTO { ID = 1, Title = "deleted movie", ReleaseDate = 2024, Runtime = TimeSpan.Parse("1:01:00") }).Result;

        // Assert
        Assert.AreEqual(expectedvalue, actualValue);
    }



    [TestMethod]
    [DataRow("", 2023, "1:02:00", "Titre requis !")]
    [DataRow(null, 2023, "1:02:00", "Titre requis !")]
    [DataRow(null, null, "1:02:00", "Titre requis !")]
    [DataRow("new Movie", null, "1:02:00", "Année de publication requis !")]
    [DataRow("new Movie", 2023, null, "Durée du Film requis !")]
    [DataRow(null, null, null, "Durée du Film requis !")]

    public void CreateWithMultipleErrors_TestMethod(string? title, int? releaseDate, string? runtime, string errorMessage)
    {
        // Arrange
        MovieDTO expectedMovie = new MovieDTO { Title = title, ReleaseDate = releaseDate, Runtime = String.IsNullOrEmpty(runtime) ? null : TimeSpan.Parse(runtime) };

        serviceCollection!.AddTransient<IMovieRepository>(provider =>
        {
            var mock = new Mock<IMovieRepository>();
            mock.Setup(repository => repository.Create(It.IsAny<Movie>())).Returns((Movie m) => Task.FromResult(m)!);
            return mock.Object;
        });
        var serviceProvider = serviceCollection!.BuildServiceProvider();
        IMovieService service = serviceProvider.GetService<IMovieService>()!;

        // Act
        Func<MovieDTO> actionMethod = () => service.Create(expectedMovie).Result;

        // Assert
        var exception = Assert.ThrowsException<AggregateException>(actionMethod);

        Assert.IsTrue(exception.Message.Contains("Movie incorrect !"));
        var innerMessages = ((System.AggregateException)exception!.InnerException!.InnerException!)
            .InnerExceptions.ToList().ConvertAll(e => e.Message);
        Assert.IsTrue(innerMessages.Contains(errorMessage));
    }
    [TestMethod]
    [DataRow("", 2023, "1:02:00", "Titre requis !")]
    [DataRow(null, 2023, "1:02:00", "Titre requis !")]
    [DataRow(null, null, "1:02:00", "Titre requis !")]
    [DataRow("new Movie", null, "1:02:00", "Année de publication requis !")]
    [DataRow("new Movie", 2023, null, "Durée du Film requis !")]
    [DataRow(null, null, null, "Durée du Film requis !")]

    public void UpdateWithMultipleErrors_TestMethod(string? title, int? releaseDate, string? runtime, string errorMessage)
    {
        // Arrange
        MovieDTO expectedMovie = new MovieDTO { Title = title, ReleaseDate = releaseDate, Runtime = String.IsNullOrEmpty(runtime) ? null : TimeSpan.Parse(runtime) };

        serviceCollection!.AddTransient<IMovieRepository>(provider =>
        {
            var mock = new Mock<IMovieRepository>();
            mock.Setup(repository => repository.Create(It.IsAny<Movie>())).Returns((Movie m) => Task.FromResult(m)!);
            return mock.Object;
        });
        var serviceProvider = serviceCollection!.BuildServiceProvider();
        IMovieService service = serviceProvider.GetService<IMovieService>()!;

        // Act
        Func<MovieDTO> actionMethod = () => service.Update(expectedMovie).Result;

        // Assert
        var exception = Assert.ThrowsException<AggregateException>(actionMethod);

        Assert.IsTrue(exception.Message.Contains("Movie incorrect !"));
        var innerMessages = ((System.AggregateException)exception!.InnerException!.InnerException!)
            .InnerExceptions.ToList().ConvertAll(e => e.Message);
        Assert.IsTrue(innerMessages.Contains(errorMessage));
    }
}