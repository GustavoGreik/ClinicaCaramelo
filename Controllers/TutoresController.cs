using ClinicaCaramelo.Data;
using ClinicaCaramelo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaCaramelo.Controllers;

[Authorize]
public class TutoresController : Controller
{
    private readonly ClinicaContext _context;
    public TutoresController(ClinicaContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Tutores.OrderBy(t => t.Nome).ToListAsync());

    public IActionResult Create() => View(new Tutor());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tutor tutor)
    {
        if (await _context.Tutores.AnyAsync(t => t.Cpf == tutor.Cpf))
            ModelState.AddModelError(nameof(tutor.Cpf), "Já existe um tutor cadastrado com este CPF.");
        if (await _context.Tutores.AnyAsync(t => t.Email == tutor.Email))
            ModelState.AddModelError(nameof(tutor.Email), "Já existe um tutor cadastrado com este e-mail.");

        if (!ModelState.IsValid) return View(tutor);
        _context.Tutores.Add(tutor);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Tutor cadastrado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var tutor = await _context.Tutores.FindAsync(id);
        return tutor == null ? NotFound() : View(tutor);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tutor tutor)
    {
        if (id != tutor.Id) return NotFound();
        if (await _context.Tutores.AnyAsync(t => t.Id != id && t.Cpf == tutor.Cpf))
            ModelState.AddModelError(nameof(tutor.Cpf), "Este CPF já está cadastrado.");
        if (await _context.Tutores.AnyAsync(t => t.Id != id && t.Email == tutor.Email))
            ModelState.AddModelError(nameof(tutor.Email), "Este e-mail já está cadastrado.");
        if (!ModelState.IsValid) return View(tutor);
        _context.Update(tutor);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Tutor atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var tutor = await _context.Tutores.FirstOrDefaultAsync(t => t.Id == id);
        return tutor == null ? NotFound() : View(tutor);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tutor = await _context.Tutores.FindAsync(id);
        if (tutor == null) return NotFound();
        if (await _context.Animais.AnyAsync(a => a.TutorId == id))
        {
            TempData["Erro"] = "Não é possível excluir este tutor porque existem animais cadastrados para ele.";
            return RedirectToAction(nameof(Index));
        }
        _context.Tutores.Remove(tutor);
        await _context.SaveChangesAsync();
        TempData["Mensagem"] = "Tutor removido.";
        return RedirectToAction(nameof(Index));
    }
}
