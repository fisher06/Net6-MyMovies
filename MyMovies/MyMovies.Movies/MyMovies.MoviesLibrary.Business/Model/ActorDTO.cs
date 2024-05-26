namespace MyMovies.MoviesLibrary.Business.Model;

public class ActorDTO
{
    public int ID { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
