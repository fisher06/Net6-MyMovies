using MyMovies.MoviesLibrary.Domain.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace MyMovies.MoviesLibrary.Domain;

public class Movie
{
    [Display(Name = "Identifiant du film")]
    public int ID { get; set; }

    [Required(ErrorMessage = "Titre requis !")]
    [DataType(DataType.Text)]
    [StringLength(4000, MinimumLength = 2, ErrorMessage = "La taille maximal est de 4000 caractères !")]
    [Display(Name = "Titre du film")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Année de publication requis !")]
    [RegularExpression(@"^(\d{4})$", ErrorMessage = "L'année est sur 4 chiffres !")]
    [MaximalDate("L'année de publication doit être inférieure à l'année en cours...")]
    [Display(Name = "Année de publication")]
    public int? ReleaseDate { get; set; }

    [Required(ErrorMessage = "Durée du Film requis !")]
    [RegularExpression(@"^(?:[0-1][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Format de la durée hh:mm:ss !")]
    [Display(Name = "Durée du Film")]
    public TimeSpan? Runtime { get; set; }

    [Display(Name = "Résumé")]
    [DataType(DataType.Text)]
    [StringLength(4000, ErrorMessage = "Le résumé ne peut comporter que 4000 caractère maxi !")]
    public string? Resume { get; set; }

    [Display(Name = "Url du Poster")]
    [DataType(DataType.Text)]
    [StringLength(4000, ErrorMessage = "l'Url du poster ne peut comporter que 4000 caractère maxi !")]
    // Uri de l'affiche du Film
    public string? ImageUri { get; set; }

    public ICollection<Producer>? Producers { get; set; }

    public ICollection<Character>? Cast { get; set; }

}
