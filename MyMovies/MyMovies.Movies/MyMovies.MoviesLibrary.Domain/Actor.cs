namespace MyMovies.MoviesLibrary.Domain;

public class Actor
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? FullName => $"{FirstName} {LastName}";
}
