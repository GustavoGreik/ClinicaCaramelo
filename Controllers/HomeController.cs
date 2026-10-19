using ClinicaCaramelo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaCaramelo.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ClinicaContext _context;
    public HomeController(ClinicaContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalTutores = await _context.Tutores.CountAsync();
        ViewBag.TotalAnimais = await _context.Animais.CountAsync();
        ViewBag.TotalVeterinarios = await _context.Veterinarios.CountAsync();
        ViewBag.ConsultasHoje = await _context.Consultas.CountAsync(c => c.DataHora.Date == DateTime.Today);
        ViewBag.ProximasConsultas = await _context.Consultas
            .Include(c => c.Animal).Include(c => c.Veterinario)
            .Where(c => c.DataHora >= DateTime.Now && c.Status == "Agendada")
            .OrderBy(c => c.DataHora).Take(5).ToListAsync();
        return View();
    }
}
