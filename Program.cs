using RecetteApp.Components;
using RecetteApp.Services;
using Microsoft.EntityFrameworkCore;
using RecetteApp.Models;
using RecetteApp.Data;
using Radzen;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(o => o.DetailedErrors = true);

builder.Services.AddScoped<IRecetteService, RecetteService>();
builder.Services.AddScoped<UserCounterService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ContextMenuService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=recette.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// ── Remplace le chemin /Account/Login par défaut d'Identity ──────────────
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath        = "/login";
    options.LogoutPath       = "/logout";
    options.AccessDeniedPath = "/login";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan   = TimeSpan.FromHours(8);
});

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (await userManager.FindByEmailAsync("admin@recette.com") == null)
    {
        var adminUser = new IdentityUser { UserName = "admin", Email = "admin@recette.com" };
        await userManager.CreateAsync(adminUser, "Admin@123");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// ── Endpoints HTTP pour auth (le cookie ne peut pas être défini via WebSocket) ──
app.MapPost("/api/auth/login", async (
    [Microsoft.AspNetCore.Mvc.FromForm] string email,
    [Microsoft.AspNetCore.Mvc.FromForm] string motDePasse,
    UserManager<IdentityUser>  userMgr,
    SignInManager<IdentityUser> signMgr) =>
{
    var user = await userMgr.FindByEmailAsync(email);
    if (user is null) return Results.Unauthorized();
    var result = await signMgr.PasswordSignInAsync(user.UserName!, motDePasse, isPersistent: true, lockoutOnFailure: false);
    return result.Succeeded ? Results.Ok() : Results.Unauthorized();
}).DisableAntiforgery();

app.MapPost("/api/auth/logout", async (SignInManager<IdentityUser> signMgr) =>
{
    await signMgr.SignOutAsync();
    return Results.Ok();
}).DisableAntiforgery();

app.Run();