using ClinicaCaramelo.Data;
using ClinicaCaramelo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaCaramelo.Controllers;

[Authorize]
public class VeterinariosController : Controller
{
    private readonly ClinicaContext _context;
    public VeterinariosController(ClinicaContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Veterinarios.OrderBy(v => v.Nome).ToListAsync());

    public IActionResult Create() => View(new Veterinario());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Veterinario veterinario)
    {
        if (await _context.Veterinarios.AnyAsync(v => v.Crmv == veterinario.Crmv))
            ModelState.AddModelError(nameof(veterinario.Crmv), "Este CRMV já está cadastrado.");
        if (!ModelState.IsValid) return View(veterinario);
        _context.Veterinarios.Add(veterinario);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Veterinário cadastrado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.Veterinarios.FindAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Veterinario veterinario)
    {
        if (id != veterinario.Id) return NotFound();
        if (await _context.Veterinarios.AnyAsync(v => v.Id != id && v.Crmv == veterinario.Crmv))
            ModelState.AddModelError(nameof(veterinario.Crmv), "Este CRMV já está cadastrado.");
        if (!ModelState.IsValid) return View(veterinario);
        _context.Update(veterinario);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Veterinário atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.Veterinarios.FirstOrDefaultAsync(v => v.Id == id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.Veterinarios.FindAsync(id);
        if (item == null) return NotFound();
        if (await _context.Consultas.AnyAsync(c => c.VeterinarioId == id))
        {
            TempData["Erro"] = "Não é possível excluir este veterinário porque existem consultas registradas.";
            return RedirectToAction(nameof(Index));
        }
        _context.Veterinarios.Remove(item);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Veterinário removido.";
        return RedirectToAction(nameof(Index));
    }
}
