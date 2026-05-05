using Microsoft.EntityFrameworkCore;
using RecetteApp.Data;
using RecetteApp.Models;

namespace RecetteApp.Services;

/// <summary>Service de gestion des utilisateurs (inscription, connexion).</summary>
public class UserCounterService
{
    private readonly AppDbContext _db;
    public UserCounterService(AppDbContext db) => _db = db;

    public async Task<List<Utilisateur>> GetAllAsync() =>
        await _db.Utilisateurs.ToListAsync();

    public async Task<Utilisateur?> GetByIdAsync(int id) =>
        await _db.Utilisateurs.FindAsync(id);

    public async Task<Utilisateur?> GetByEmailAsync(string email) =>
        await _db.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> EmailExisteAsync(string email) =>
        await _db.Utilisateurs.AnyAsync(u => u.Email == email);

    public async Task<Utilisateur> InscrireAsync(string nom, string email, string motDePasse)
    {
        if (await EmailExisteAsync(email))
            throw new InvalidOperationException("Cet email est déjà utilisé.");

        var user = new Utilisateur { Nom = nom, Email = email, MotDePasse = motDePasse };
        _db.Utilisateurs.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<Utilisateur?> ConnecterAsync(string email, string motDePasse) =>
        await _db.Utilisateurs
            .FirstOrDefaultAsync(u => u.Email == email && u.MotDePasse == motDePasse);

    public async Task SupprimerAsync(int id)
    {
        var u = await _db.Utilisateurs.FindAsync(id);
        if (u is not null) { _db.Utilisateurs.Remove(u); await _db.SaveChangesAsync(); }
    }

    public async Task<int> CountAsync() => await _db.Utilisateurs.CountAsync();
}
