using RecetteApp.Models;

namespace RecetteApp.Services;

public interface IRecetteService
{
    // ── Ingrédients ──────────────────────────────────────────────────────────
    Task<List<Ingredient>> GetAllIngredientsAsync();
    Task<Ingredient?>      GetIngredientByIdAsync(int id);
    Task<Ingredient>       CreateIngredientAsync(Ingredient i);
    Task                   UpdateIngredientAsync(Ingredient i);
    Task                   DeleteIngredientAsync(int id);
    Task<List<Ingredient>> SearchIngredientsAsync(string terme);

    // ── Recettes ─────────────────────────────────────────────────────────────
    Task<List<Recette>> GetAllRecettesAsync();
    Task<Recette?>      GetRecetteByIdAsync(int id);
    Task<Recette>       CreateRecetteAsync(Recette r);
    Task                UpdateRecetteAsync(Recette r);
    Task                DeleteRecetteAsync(int id);
    Task<List<Recette>> SearchRecettesAsync(string terme);
    Task<List<Recette>> GetRecettesByCategorieAsync(int categorieId);
    Task<List<Recette>> GetRecettesByTypeCuisineAsync(int typeId);

    // ── Gestion des ingrédients d'une recette ─────────────────────────────
    Task AjouterIngredientAsync(int recetteId, int ingredientId, float quantite);
    Task RetirerIngredientAsync(int recetteIngredientId);
    Task MettreAJourQuantiteAsync(int recetteIngredientId, float quantite);

    // ── Catégories & Types ───────────────────────────────────────────────────
    Task<List<Categorie>>   GetAllCategoriesAsync();
    Task<List<TypeCuisine>> GetAllTypesCuisineAsync();

    // ── Analyse ──────────────────────────────────────────────────────────────
    Task<float>              TotalCaloriesAsync(int recetteId);
    Task<float>              CaloriesParPersonneAsync(int recetteId);
    Task<Dictionary<string,int>> RepartitionCategorieAsync();
    Task<Dictionary<string,int>> RepartitionTypeCuisineAsync();
    Task<List<Recette>>      TopCaloriquesAsync(int n = 5);
    Task<StatistiquesGlobales> GetStatsAsync();
}

public record StatistiquesGlobales
{
    public int    TotalRecettes    { get; init; }
    public int    TotalIngredients { get; init; }
    public float  MoyenneCalories  { get; init; }
    public string RecetteTop       { get; init; } = "-";
}