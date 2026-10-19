using ClinicaCaramelo.Data;
using ClinicaCaramelo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicaCaramelo.Controllers;

[Authorize]
public class AnimaisController : Controller
{
    private readonly ClinicaContext _context;
    public AnimaisController(ClinicaContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Animais.Include(a => a.Tutor).OrderBy(a => a.Nome).ToListAsync());

    public async Task<IActionResult> Create()
    {
        await CarregarTutores();
        return View(new Animal());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Animal animal)
    {
        if (!await _context.Tutores.AnyAsync(t => t.Id == animal.TutorId))
            ModelState.AddModelError(nameof(animal.TutorId), "Selecione um tutor válido.");
        if (!ModelState.IsValid)
        {
            await CarregarTutores(animal.TutorId);
            return View(animal);
        }
        _context.Animais.Add(animal);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Animal cadastrado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var animal = await _context.Animais.FindAsync(id);
        if (animal == null) return NotFound();
        await CarregarTutores(animal.TutorId);
        return View(animal);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Animal animal)
    {
        if (id != animal.Id) return NotFound();
        if (!await _context.Tutores.AnyAsync(t => t.Id == animal.TutorId))
            ModelState.AddModelError(nameof(animal.TutorId), "Selecione um tutor válido.");
        if (!ModelState.IsValid)
        {
            await CarregarTutores(animal.TutorId);
            return View(animal);
        }
        _context.Update(animal);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Animal atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var animal = await _context.Animais.Include(a => a.Tutor).FirstOrDefaultAsync(a => a.Id == id);
        return animal == null ? NotFound() : View(animal);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var animal = await _context.Animais.FindAsync(id);
        if (animal == null) return NotFound();
        if (await _context.Consultas.AnyAsync(c => c.AnimalId == id))
        {
            TempData["Erro"] = "Não é possível excluir este animal porque ele possui consultas registradas.";
            return RedirectToAction(nameof(Index));
        }
        _context.Animais.Remove(animal);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Animal removido.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CarregarTutores(int? selecionado = null)
    {
        ViewBag.TutorId = new SelectList(await _context.Tutores.OrderBy(t => t.Nome).ToListAsync(), "Id", "Nome", selecionado);
    }
}
