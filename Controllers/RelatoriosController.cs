using ClinicaCaramelo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaCaramelo.Controllers;

[Authorize]
public class RelatoriosController : Controller
{
    private readonly ClinicaContext _context;
    public RelatoriosController(ClinicaContext context) => _context = context;

    public async Task<IActionResult> Consulta(int? id)
    {
        if (id == null) return NotFound();
        var consulta = await _context.Consultas
            .Include(c => c.Animal).ThenInclude(a => a!.Tutor)
            .Include(c => c.Veterinario)
            .FirstOrDefaultAsync(c => c.Id == id);
        return consulta == null ? NotFound() : View(consulta);
    }
}
