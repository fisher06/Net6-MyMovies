namespace MyMovies.MoviesLibrary.Domain
{
    public class Character
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public Actor? Actor { get; set; }
    }
}
