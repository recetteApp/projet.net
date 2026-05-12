namespace RecetteApp.Models;

/// <summary>
/// Classe utilitaire d'analyse nutritionnelle et statistique sur un ensemble de recettes.
/// </summary>
public class AnalyseRecette
{
    private readonly IReadOnlyList<Recette> _recettes;

    public AnalyseRecette(IEnumerable<Recette> recettes)
    {
        _recettes = recettes.ToList();
    }

    /// <summary>Retourne les N recettes les plus caloriques (total).</summary>
    public IEnumerable<Recette> TopRecettesCaloriques(int n = 5)
        => _recettes.OrderByDescending(r => r.GetCaloriesTotales()).Take(n);

    /// <summary>Répartition du nombre de recettes par catégorie.</summary>
    public Dictionary<string, int> RepartitionCategorie()
        => _recettes
            .GroupBy(r => r.Categorie?.Nom ?? "Inconnue")
            .ToDictionary(g => g.Key, g => g.Count());

    /// <summary>Répartition du nombre de recettes par type de cuisine.</summary>
    public Dictionary<string, int> RepartitionTypeCuisine()
        => _recettes
            .GroupBy(r => r.TypeCuisine?.Nom ?? "Inconnue")
            .ToDictionary(g => g.Key, g => g.Count());

    /// <summary>Moyenne de calories par personne sur toutes les recettes.</summary>
    public float MoyenneCaloriesParPersonne()
        => _recettes.Count > 0
            ? _recettes.Average(r => r.GetCaloriesParPersonne())
            : 0f;
}
