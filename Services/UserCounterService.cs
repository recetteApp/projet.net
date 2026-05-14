using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecetteApp.Data;

namespace RecetteApp.Services;

/// <summary>Service de gestion des utilisateurs via ASP.NET Identity.</summary>
public class UserCounterService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UserCounterService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager    = userManager;
        _signInManager  = signInManager;
    }

    public async Task<List<IdentityUser>> GetAllAsync() =>
        await _userManager.Users.ToListAsync();

    public async Task<IdentityUser?> GetByIdAsync(string id) =>
        await _userManager.FindByIdAsync(id);

    public async Task<IdentityUser?> GetByEmailAsync(string email) =>
        await _userManager.FindByEmailAsync(email);

    public async Task<bool> EmailExisteAsync(string email) =>
        await _userManager.FindByEmailAsync(email) is not null;

    public async Task<IdentityUser> InscrireAsync(string nom, string email, string motDePasse)
    {
        if (await EmailExisteAsync(email))
            throw new InvalidOperationException("Cet email est déjà utilisé.");

        var user = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, motDePasse);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Erreur lors de l'inscription : {errors}");
        }

        return user;
    }

    public async Task<bool> ConnecterAsync(string email, string motDePasse)
    {
        // PasswordSignInAsync utilise le UserName, pas l'email
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName!, motDePasse, isPersistent: true, lockoutOnFailure: false);
        return result.Succeeded;
    }

    public async Task DeconnecterAsync() =>
        await _signInManager.SignOutAsync();

    public async Task SupprimerAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is not null)
            await _userManager.DeleteAsync(user);
    }

    public async Task<int> CountAsync() =>
        await _userManager.Users.CountAsync();
}