using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetteApp.Models;

public class Ingredient
{
    [Key] public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Nom { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Unite { get; set; } = string.Empty;

    [Column(TypeName = "REAL"), Range(0, float.MaxValue)]
    public float CaloriesParUnite { get; set; }

    public ICollection<RecetteIngredient> RecetteIngredients { get; set; } = new List<RecetteIngredient>();

    public float GetCalories() => CaloriesParUnite;
}