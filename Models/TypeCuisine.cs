using System.ComponentModel.DataAnnotations;

namespace RecetteApp.Models;

public class TypeCuisine
{
    [Key] 
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom du type de cuisine est obligatoire.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit faire entre 2 et 100 caractères.")]
    public string Nom { get; set; } = string.Empty;

    public ICollection<Recette> Recettes { get; set; } = new List<Recette>();

    public string GetNomCuisine() => Nom;
}