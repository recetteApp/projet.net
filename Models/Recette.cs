using System.ComponentModel.DataAnnotations;

namespace RecetteApp.Models;

public class Recette
{
    [Key] 
    public int Id { get; set; }

    [Required(ErrorMessage = "Le titre est obligatoire.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Le titre doit faire entre 3 et 200 caractères.")]
    public string Titre { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères.")]
    public string Description { get; set; } = string.Empty;

    [Range(1, 50, ErrorMessage = "Le nombre de personnes doit être entre 1 et 50.")]
    public int NbPersonnes { get; set; } = 4;

    public DateTime DateCreation { get; set; } = DateTime.Now;

    [StringLength(2000, ErrorMessage = "Les étapes ne peuvent pas dépasser 2000 caractères.")]
    public string Etapes { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner une catégorie valide.")]
    public int CategorieId { get; set; }
    public Categorie Categorie { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un type de cuisine valide.")]
    public int TypeCuisineId { get; set; }
    public TypeCuisine TypeCuisine { get; set; } = null!;

    public ICollection<RecetteIngredient> Ingredients { get; set; } = new List<RecetteIngredient>();

    public float GetCaloriesTotales()     => Ingredients.Sum(ri => ri.GetCaloriesTotales());
    public float GetCaloriesParPersonne() => NbPersonnes > 0 ? GetCaloriesTotales() / NbPersonnes : 0f;
    public void  AjouterIngredient(RecetteIngredient ri) { ri.RecetteId = Id; Ingredients.Add(ri); }
    public void  RetirerIngredient(RecetteIngredient ri) => Ingredients.Remove(ri);
}