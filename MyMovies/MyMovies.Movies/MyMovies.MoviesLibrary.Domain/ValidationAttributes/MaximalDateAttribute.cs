
using System.ComponentModel.DataAnnotations;

namespace MyMovies.MoviesLibrary.Domain.ValidationAttributes;

public class MaximalDateAttribute : ValidationAttribute
{
    public MaximalDateAttribute(string errorMessage) : base(errorMessage)
    {
    }

    public override bool IsValid(object? value)
    {
        int year = 0000;
        return Int32.TryParse(Convert.ToString(value), out year) && year <= DateTime.Now.Year;
    }
}
