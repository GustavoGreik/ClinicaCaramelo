using ClinicaCaramelo.Data;
using ClinicaCaramelo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicaCaramelo.Controllers;

[Authorize]
public class ConsultasController : Controller
{
    private readonly ClinicaContext _context;
    public ConsultasController(ClinicaContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Consultas.Include(c => c.Animal).Include(c => c.Veterinario)
            .OrderByDescending(c => c.DataHora).ToListAsync());

    public async Task<IActionResult> Create()
    {
        await CarregarOpcoes();
        return View(new Consulta { DataHora = DateTime.Now.AddHours(1) });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Consulta consulta)
    {
        await ValidarRelacionamentos(consulta);
        if (consulta.DataHora < DateTime.Now.AddMinutes(-5))
            ModelState.AddModelError(nameof(consulta.DataHora), "Escolha uma data futura.");
        if (!ModelState.IsValid)
        {
            await CarregarOpcoes(consulta.AnimalId, consulta.VeterinarioId);
            return View(consulta);
        }
        _context.Consultas.Add(consulta);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Consulta agendada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var consulta = await _context.Consultas.FindAsync(id);
        if (consulta == null) return NotFound();
        await CarregarOpcoes(consulta.AnimalId, consulta.VeterinarioId);
        return View(consulta);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Consulta consulta)
    {
        if (id != consulta.Id) return NotFound();
        await ValidarRelacionamentos(consulta);
        if (!ModelState.IsValid)
        {
            await CarregarOpcoes(consulta.AnimalId, consulta.VeterinarioId);
            return View(consulta);
        }
        _context.Update(consulta);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Consulta atualizada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var consulta = await _context.Consultas.Include(c => c.Animal).Include(c => c.Veterinario)
            .FirstOrDefaultAsync(c => c.Id == id);
        return consulta == null ? NotFound() : View(consulta);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);
        if (consulta == null) return NotFound();
        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Consulta removida.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarRelacionamentos(Consulta consulta)
    {
        if (!await _context.Animais.AnyAsync(a => a.Id == consulta.AnimalId))
            ModelState.AddModelError(nameof(consulta.AnimalId), "Selecione um animal válido.");
        if (!await _context.Veterinarios.AnyAsync(v => v.Id == consulta.VeterinarioId))
            ModelState.AddModelError(nameof(consulta.VeterinarioId), "Selecione um veterinário válido.");
    }

    private async Task CarregarOpcoes(int? animalId = null, int? veterinarioId = null)
    {
        ViewBag.AnimalId = new SelectList(await _context.Animais.OrderBy(a => a.Nome).ToListAsync(), "Id", "Nome", animalId);
        ViewBag.VeterinarioId = new SelectList(await _context.Veterinarios.OrderBy(v => v.Nome).ToListAsync(), "Id", "Nome", veterinarioId);
    }
}
