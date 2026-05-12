using Microsoft.EntityFrameworkCore;
using RecetteApp.Data;
using RecetteApp.Models;

namespace RecetteApp.Services;

public class RecetteService : IRecetteService
{
    private readonly AppDbContext _db;
    public RecetteService(AppDbContext db) => _db = db;

    // ── Helpers ───────────────────────────────────────────────────────────────
    private IQueryable<Recette> RecettesAvecIncludes() =>
        _db.Recettes
           .Include(r => r.Categorie)
           .Include(r => r.TypeCuisine)
           .Include(r => r.Ingredients).ThenInclude(ri => ri.Ingredient);

    // ── Ingrédients ───────────────────────────────────────────────────────────
    public async Task<List<Ingredient>> GetAllIngredientsAsync() =>
        await _db.Ingredients.OrderBy(i => i.Nom).ToListAsync();

    public async Task<Ingredient?> GetIngredientByIdAsync(int id) =>
        await _db.Ingredients.FindAsync(id);

    public async Task<Ingredient> CreateIngredientAsync(Ingredient i)
    {
        _db.Ingredients.Add(i);
        await _db.SaveChangesAsync();
        return i;
    }

    public async Task UpdateIngredientAsync(Ingredient i)
    {
        _db.Ingredients.Update(i);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteIngredientAsync(int id)
    {
        var i = await _db.Ingredients.FindAsync(id);
        if (i is not null) { _db.Ingredients.Remove(i); await _db.SaveChangesAsync(); }
    }

    public async Task<List<Ingredient>> SearchIngredientsAsync(string terme) =>
        await _db.Ingredients.Where(i => i.Nom.Contains(terme)).OrderBy(i => i.Nom).ToListAsync();

    // ── Recettes ──────────────────────────────────────────────────────────────
    public async Task<List<Recette>> GetAllRecettesAsync() =>
        await RecettesAvecIncludes().OrderByDescending(r => r.DateCreation).ToListAsync();

    public async Task<Recette?> GetRecetteByIdAsync(int id) =>
        await RecettesAvecIncludes().FirstOrDefaultAsync(r => r.Id == id);

    public async Task<Recette> CreateRecetteAsync(Recette r)
    {
        _db.Recettes.Add(r);
        await _db.SaveChangesAsync();
        return r;
    }

    public async Task UpdateRecetteAsync(Recette r)
    {
        _db.Recettes.Update(r);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteRecetteAsync(int id)
    {
        var r = await _db.Recettes.FindAsync(id);
        if (r is not null) { _db.Recettes.Remove(r); await _db.SaveChangesAsync(); }
    }

    public async Task<List<Recette>> SearchRecettesAsync(string terme) =>
        await RecettesAvecIncludes()
            .Where(r => r.Titre.Contains(terme) || r.Description.Contains(terme))
            .ToListAsync();

    public async Task<List<Recette>> GetRecettesByCategorieAsync(int categorieId) =>
        await RecettesAvecIncludes().Where(r => r.CategorieId == categorieId).ToListAsync();

    public async Task<List<Recette>> GetRecettesByTypeCuisineAsync(int typeId) =>
        await RecettesAvecIncludes().Where(r => r.TypeCuisineId == typeId).ToListAsync();

    // ── Gestion ingrédients d'une recette ─────────────────────────────────────
    public async Task AjouterIngredientAsync(int recetteId, int ingredientId, float quantite)
    {
        var existe = await _db.RecetteIngredients
            .AnyAsync(ri => ri.RecetteId == recetteId && ri.IngredientId == ingredientId);
        if (!existe)
        {
            _db.RecetteIngredients.Add(new RecetteIngredient
                { RecetteId = recetteId, IngredientId = ingredientId, Quantite = quantite });
            await _db.SaveChangesAsync();
        }
    }

    public async Task RetirerIngredientAsync(int riId)
    {
        var ri = await _db.RecetteIngredients.FindAsync(riId);
        if (ri is not null) { _db.RecetteIngredients.Remove(ri); await _db.SaveChangesAsync(); }
    }

    public async Task MettreAJourQuantiteAsync(int riId, float qte)
    {
        var ri = await _db.RecetteIngredients.FindAsync(riId);
        if (ri is not null) { ri.Quantite = qte; await _db.SaveChangesAsync(); }
    }

    // ── Catégories & Types ────────────────────────────────────────────────────
    public async Task<List<Categorie>>   GetAllCategoriesAsync()   => await _db.Categories.ToListAsync();
    public async Task<List<TypeCuisine>> GetAllTypesCuisineAsync() => await _db.TypesCuisine.ToListAsync();

    // ── Analyse ───────────────────────────────────────────────────────────────
    private async Task<List<Recette>> ToutesAvecIncludes() =>
        await RecettesAvecIncludes().ToListAsync();

    public async Task<float> TotalCaloriesAsync(int recetteId)
    {
        var r = await RecettesAvecIncludes().FirstOrDefaultAsync(r => r.Id == recetteId);
        return r?.GetCaloriesTotales() ?? 0f;
    }

    public async Task<float> CaloriesParPersonneAsync(int recetteId)
    {
        var r = await RecettesAvecIncludes().FirstOrDefaultAsync(r => r.Id == recetteId);
        return r?.GetCaloriesParPersonne() ?? 0f;
    }

    public async Task<Dictionary<string, int>> RepartitionCategorieAsync()
    {
        var list = await ToutesAvecIncludes();
        return new AnalyseRecette(list).RepartitionCategorie();
    }

    public async Task<Dictionary<string, int>> RepartitionTypeCuisineAsync()
    {
        var list = await ToutesAvecIncludes();
        return new AnalyseRecette(list).RepartitionTypeCuisine();
    }

    public async Task<List<Recette>> TopCaloriquesAsync(int n = 5)
    {
        var list = await ToutesAvecIncludes();
        return new AnalyseRecette(list).TopRecettesCaloriques(n).ToList();
    }

    public async Task<StatistiquesGlobales> GetStatsAsync()
    {
        var list = await ToutesAvecIncludes();
        return new StatistiquesGlobales
        {
            TotalRecettes    = list.Count,
            TotalIngredients = await _db.Ingredients.CountAsync(),
            MoyenneCalories  = list.Count > 0 ? list.Average(r => r.GetCaloriesTotales()) : 0f,
            RecetteTop       = list.OrderByDescending(r => r.GetCaloriesTotales())
                                   .FirstOrDefault()?.Titre ?? "-"
        };
    }
}