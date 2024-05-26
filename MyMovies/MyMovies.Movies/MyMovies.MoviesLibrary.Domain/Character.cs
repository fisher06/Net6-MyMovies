using System.ComponentModel.DataAnnotations;

namespace MyMovies.MoviesLibrary.Domain;

public class Character
{
    [Display(Name = "Identifiant de personnage")]
    public int ID { get; set; }

    [Required(ErrorMessage = "Nom du personnage requis !")]
    [DataType(DataType.Text)]
    [StringLength(4000, MinimumLength = 2, ErrorMessage = "La taille maximal est de 4000 caractères !")]
    public string? Name { get; set; }

    public Actor? Actor { get; set; }

}
