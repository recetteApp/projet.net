using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetteApp.Models;

public class RecetteIngredient
{
    [Key] 
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner une recette valide.")]
    public int RecetteId { get; set; }
    public Recette Recette { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un ingrédient valide.")]
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;

    [Column(TypeName = "REAL")]
    [Range(0.01, float.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0.")]
    public float Quantite { get; set; }

    public float GetCaloriesTotales() => Quantite * (Ingredient?.CaloriesParUnite ?? 0f);
}