namespace MyMovies.MoviesLibrary.Domain
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int ReleaseDate { get; set; }
        public TimeSpan Runtime { get; set; }
        public string? Resume { get; set; }
        public string? ImageUri { get; set; }
        public List<Producer>? Producers { get; set; }
        public List<Character>? Cast { get; set; }
    }
}
