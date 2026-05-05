using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetteApp.Models;

public class RecetteIngredient
{
    [Key] public int Id { get; set; }

    public int RecetteId { get; set; }
    public Recette Recette { get; set; } = null!;

    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;

    [Column(TypeName = "REAL"), Range(0.01, float.MaxValue)]
    public float Quantite { get; set; }

    public float GetCaloriesTotales() => Quantite * (Ingredient?.CaloriesParUnite ?? 0f);
}