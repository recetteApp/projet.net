using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetteApp.Models;

public class Ingredient
{
    [Key] 
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom de l'ingrédient est obligatoire.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Le nom doit faire entre 2 et 150 caractères.")]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'unité est obligatoire.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "L'unité doit faire entre 1 et 50 caractères.")]
    public string Unite { get; set; } = string.Empty;

    [Column(TypeName = "REAL")]
    [Range(0, float.MaxValue, ErrorMessage = "Les calories doivent être supérieures à 0.")]
    public float CaloriesParUnite { get; set; }

    public ICollection<RecetteIngredient> RecetteIngredients { get; set; } = new List<RecetteIngredient>();

    public float GetCalories() => CaloriesParUnite;
}