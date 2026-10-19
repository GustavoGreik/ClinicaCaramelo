using System.Security.Claims;
using ClinicaCaramelo.Data;
using ClinicaCaramelo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaCaramelo.Controllers;

public class LoginController : Controller
{
    private readonly ClinicaContext _context;
    public LoginController(ClinicaContext context) => _context = context;

    [HttpGet]
    public IActionResult Index(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginViewModel model, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (!ModelState.IsValid) return View(model);
        var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == model.Email);
        if (usuario == null || new PasswordHasher<Usuario>().VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha) == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email)
        };
        var identity = new ClaimsIdentity(claims, "CarameloCookie");
        await HttpContext.SignInAsync("CarameloCookie", new ClaimsPrincipal(identity));
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Sair()
    {
        await HttpContext.SignOutAsync("CarameloCookie");
        return RedirectToAction(nameof(Index));
    }
}
