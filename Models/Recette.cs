using System.ComponentModel.DataAnnotations;

namespace RecetteApp.Models;

public class Recette
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Titre { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Range(1, 100)]
    public int NbPersonnes { get; set; } = 4;

    public DateTime DateCreation { get; set; } = DateTime.Now;

    public string Etapes { get; set; } = string.Empty;

    public int CategorieId { get; set; }
    public Categorie Categorie { get; set; } = null!;

    public int TypeCuisineId { get; set; }
    public TypeCuisine TypeCuisine { get; set; } = null!;

    public int? UtilisateurId { get; set; }
    public Utilisateur? Utilisateur { get; set; }

    public ICollection<RecetteIngredient> Ingredients { get; set; } = new List<RecetteIngredient>();

    public float GetCaloriesTotales()     => Ingredients.Sum(ri => ri.GetCaloriesTotales());
    public float GetCaloriesParPersonne() => NbPersonnes > 0 ? GetCaloriesTotales() / NbPersonnes : 0f;
    public void  AjouterIngredient(RecetteIngredient ri) { ri.RecetteId = Id; Ingredients.Add(ri); }
    public void  RetirerIngredient(RecetteIngredient ri) => Ingredients.Remove(ri);
}