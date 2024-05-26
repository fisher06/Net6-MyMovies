using System.ComponentModel.DataAnnotations;

namespace MyMovies.MoviesLibrary.Domain;

public class Actor
{
    [Display(Name = "Identifiant de l'acteur")]
    public int ID { get; set; }

    [Required(ErrorMessage = "Prénom requis !")]
    [DataType(DataType.Text)]
    [StringLength(4000, MinimumLength = 2, ErrorMessage = "La taille maximal est de 4000 caractères !")]
    [Display(Name = "Prénom")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Nom requis !")]
    [DataType(DataType.Text)]
    [StringLength(4000, MinimumLength = 2, ErrorMessage = "La taille maximal est de 4000 caractères !")]
    [Display(Name = "Nom")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Date de naissance requis !")]
    [DataType(DataType.Date)]
    [Display(Name = "Date de naissance")]
    public DateTime? BirthDate { get; set; }

    public string FullName => $"{FirstName} {LastName}";

}
