using Microsoft.EntityFrameworkCore;
using RecetteApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RecetteApp.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ingredient>        Ingredients        => Set<Ingredient>();
    public DbSet<Categorie>         Categories         => Set<Categorie>();
    public DbSet<TypeCuisine>       TypesCuisine       => Set<TypeCuisine>();
    public DbSet<Recette>           Recettes           => Set<Recette>();
    public DbSet<RecetteIngredient> RecetteIngredients => Set<RecetteIngredient>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // ── Relations ────────────────────────────────────────────────────────
        mb.Entity<RecetteIngredient>()
            .HasOne(ri => ri.Recette).WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.RecetteId).OnDelete(DeleteBehavior.Cascade);

        mb.Entity<RecetteIngredient>()
            .HasOne(ri => ri.Ingredient).WithMany(i => i.RecetteIngredients)
            .HasForeignKey(ri => ri.IngredientId).OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Recette>()
            .HasOne(r => r.Categorie).WithMany(c => c.Recettes)
            .HasForeignKey(r => r.CategorieId).OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Recette>()
            .HasOne(r => r.TypeCuisine).WithMany(t => t.Recettes)
            .HasForeignKey(r => r.TypeCuisineId).OnDelete(DeleteBehavior.Restrict);

        // ── Seed : Catégories ────────────────────────────────────────────────
        mb.Entity<Categorie>().HasData(
            new Categorie { Id = 1, Nom = "Petit-déjeuner" },
            new Categorie { Id = 2, Nom = "Déjeuner"       },
            new Categorie { Id = 3, Nom = "Dîner"          },
            new Categorie { Id = 4, Nom = "Dessert"        });

        // ── Seed : Types de cuisine ──────────────────────────────────────────
        mb.Entity<TypeCuisine>().HasData(
            new TypeCuisine { Id = 1, Nom = "Tunisienne"      },
            new TypeCuisine { Id = 2, Nom = "Française"       },
            new TypeCuisine { Id = 3, Nom = "Italienne"       },
            new TypeCuisine { Id = 4, Nom = "Méditerranéenne" },
            new TypeCuisine { Id = 5, Nom = "Orientale"       },
            new TypeCuisine { Id = 6, Nom = "Internationale"  });

        // ── Seed : Ingrédients ───────────────────────────────────────────────
        mb.Entity<Ingredient>().HasData(
            new Ingredient { Id =  1, Nom = "Farine de blé",    Unite = "g",      CaloriesParUnite = 3.64f },
            new Ingredient { Id =  2, Nom = "Huile d'olive",    Unite = "ml",     CaloriesParUnite = 8.84f },
            new Ingredient { Id =  3, Nom = "Oeuf",             Unite = "pièce",  CaloriesParUnite = 78f   },
            new Ingredient { Id =  4, Nom = "Sucre",            Unite = "g",      CaloriesParUnite = 3.87f },
            new Ingredient { Id =  5, Nom = "Lait entier",      Unite = "ml",     CaloriesParUnite = 0.61f },
            new Ingredient { Id =  6, Nom = "Beurre",           Unite = "g",      CaloriesParUnite = 7.17f },
            new Ingredient { Id =  7, Nom = "Poulet",           Unite = "g",      CaloriesParUnite = 1.65f },
            new Ingredient { Id =  8, Nom = "Tomate",           Unite = "g",      CaloriesParUnite = 0.18f },
            new Ingredient { Id =  9, Nom = "Pates",            Unite = "g",      CaloriesParUnite = 3.71f },
            new Ingredient { Id = 10, Nom = "Riz",              Unite = "g",      CaloriesParUnite = 3.60f },
            new Ingredient { Id = 11, Nom = "Oignon",           Unite = "g",      CaloriesParUnite = 0.40f },
            new Ingredient { Id = 12, Nom = "Ail",              Unite = "gousse", CaloriesParUnite = 4.5f  },
            new Ingredient { Id = 13, Nom = "Sel",              Unite = "g",        CaloriesParUnite = 0f    },
            new Ingredient { Id = 14, Nom = "Poivre",           Unite = "g",        CaloriesParUnite = 2.51f },
            new Ingredient { Id = 15, Nom = "Concentre tomate", Unite = "g",        CaloriesParUnite = 0.82f },
            new Ingredient { Id = 16, Nom = "Huile végétale",   Unite = "cuillère", CaloriesParUnite = 45f   },
            new Ingredient { Id = 17, Nom = "Persil",           Unite = "g",        CaloriesParUnite = 0.36f },
            new Ingredient { Id = 18, Nom = "Coriandre",        Unite = "g",        CaloriesParUnite = 0.23f },
            new Ingredient { Id = 19, Nom = "Cumin",            Unite = "cuillère", CaloriesParUnite = 22.5f },
            new Ingredient { Id = 20, Nom = "Paprika",          Unite = "cuillère", CaloriesParUnite = 17f   },
            new Ingredient { Id = 21, Nom = "Curcuma",          Unite = "cuillère", CaloriesParUnite = 12.5f },
            new Ingredient { Id = 22, Nom = "Cannelle",         Unite = "cuillère", CaloriesParUnite = 10.5f },
            new Ingredient { Id = 23, Nom = "Gingembre",        Unite = "cuillère", CaloriesParUnite = 8f    },
            new Ingredient { Id = 24, Nom = "Carotte",          Unite = "g",        CaloriesParUnite = 0.41f },
            new Ingredient { Id = 25, Nom = "Pomme de terre",   Unite = "g",        CaloriesParUnite = 0.77f },
            new Ingredient { Id = 26, Nom = "Courgette",        Unite = "g",        CaloriesParUnite = 0.17f },
            new Ingredient { Id = 27, Nom = "Aubergine",        Unite = "g",        CaloriesParUnite = 0.25f },
            new Ingredient { Id = 28, Nom = "Poivron",          Unite = "g",        CaloriesParUnite = 0.31f },
            new Ingredient { Id = 29, Nom = "Fromage râpé",     Unite = "g",        CaloriesParUnite = 4.02f },
            new Ingredient { Id = 30, Nom = "Crème fraîche",    Unite = "ml",       CaloriesParUnite = 2.95f });
    }
}