using System.ComponentModel.DataAnnotations;

namespace RecetteApp.Models;

public class Categorie
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nom { get; set; } = string.Empty;

    public ICollection<Recette> Recettes { get; set; } = new List<Recette>();

    public string GetNomCategorie() => Nom;
}