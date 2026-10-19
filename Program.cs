using ClinicaCaramelo.Data;
using ClinicaCaramelo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("A connection string 'DefaultConnection' não foi configurada.");

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ClinicaContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddAuthentication("CarameloCookie")
    .AddCookie("CarameloCookie", options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/Index";
        options.Cookie.Name = "ClinicaCaramelo.Auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Cria uma conta demonstrativa caso ainda não exista. Troque a senha antes de usar fora de aula.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClinicaContext>();
    db.Database.EnsureCreated();
    const string emailInicial = "admin@caramelo.com.br";
    if (!db.Usuarios.Any(u => u.Email == emailInicial))
    {
        var usuario = new Usuario { Nome = "Administrador", Email = emailInicial };
        usuario.SenhaHash = new PasswordHasher<Usuario>().HashPassword(usuario, "Caramelo@123");
        db.Usuarios.Add(usuario);
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
