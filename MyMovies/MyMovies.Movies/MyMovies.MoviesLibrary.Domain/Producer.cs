using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.MoviesLibrary.Domain;

public class Producer
{
    [Display(Name = "Identifiant du producteur")]
    [Column("ProcuderID")]
    public int ID { get; set; }

    [Required(ErrorMessage = "Prénom requis !")]
    [DataType(DataType.Text)]
    [StringLength(4000, MinimumLength = 2, ErrorMessage = "La taille maximal est de 4000 caractères !")]
    [Display(Name = "Prénom")]
    [Column("ProducerFirstName")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Nom requis !")]
    [DataType(DataType.Text)]
    [StringLength(4000, MinimumLength = 2, ErrorMessage = "La taille maximal est de 4000 caractères !")]
    [Display(Name = "Nom")]
    [Column("ProducerLastName")]
    public string? LastName { get; set; }

}
