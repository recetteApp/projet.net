using System.ComponentModel.DataAnnotations;

namespace RecetteApp.Models;

public class Utilisateur
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Required, MaxLength(200), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string MotDePasse { get; set; } = string.Empty;

    public ICollection<Recette> Recettes { get; set; } = new List<Recette>();

    public void SupprimerRecette(Recette r) => Recettes.Remove(r);
}