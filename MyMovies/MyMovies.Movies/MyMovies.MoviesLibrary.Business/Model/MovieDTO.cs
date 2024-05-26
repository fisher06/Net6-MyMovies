namespace MyMovies.MoviesLibrary.Business.Model;

public class MovieDTO
{

    public int ID { get; set; }

    public string? Title { get; set; }

    public int? ReleaseDate { get; set; }

    public TimeSpan? Runtime { get; set; }

    public string? Resume { get; set; }

    // Uri de l'affiche du Film
    public string? ImageUri { get; set; }

    public ICollection<ProducerDTO>? Producers { get; set; }

    public ICollection<CharacterDTO>? Cast { get; set; }

}
