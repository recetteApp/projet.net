namespace RecetteApp.Models;

public class AnalyseRecette
{
    public float  CalTotales     { get; private set; }
    public float  CalParPersonne { get; private set; }
    public string Categorie      { get; private set; } = string.Empty;
    public string TypeCuisine    { get; private set; } = string.Empty;

    private readonly IEnumerable<Recette> _recettes;
    public AnalyseRecette(IEnumerable<Recette> recettes) => _recettes = recettes;

    public void Calculer(Recette r)
    {
        CalTotales     = r.GetCaloriesTotales();
        CalParPersonne = r.GetCaloriesParPersonne();
        Categorie      = r.Categorie?.Nom ?? "";
        TypeCuisine    = r.TypeCuisine?.Nom ?? "";
    }

    public Dictionary<string, int> RepartitionCategorie() =>
        _recettes.GroupBy(r => r.Categorie?.Nom ?? "Inconnu").ToDictionary(g => g.Key, g => g.Count());

    public Dictionary<string, int> RepartitionTypeCuisine() =>
        _recettes.GroupBy(r => r.TypeCuisine?.Nom ?? "Inconnu").ToDictionary(g => g.Key, g => g.Count());

    public IEnumerable<Recette> TopRecettesCaloriques(int n) =>
        _recettes.OrderByDescending(r => r.GetCaloriesTotales()).Take(n);
}