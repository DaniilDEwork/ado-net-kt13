using Kt13Library.Data;
using Kt13Library.Models;
using Kt13Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kt13Library.Controllers;

[Route("Authors")]
public class AuthorsController(AppDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await db.Authors.AsNoTracking().Include(author => author.Books).OrderBy(author => author.AuthorId).ToListAsync());
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var author = await db.Authors.AsNoTracking().Include(author => author.Books).FirstOrDefaultAsync(author => author.AuthorId == id);
        return author is null ? NotFound() : View(author);
    }

    [HttpGet("Create")]
    public IActionResult Create() => View(new AuthorForm());

    [HttpPost("Create")]
    public async Task<IActionResult> Create(AuthorForm model)
    {
        if (!ModelState.IsValid) return View(model);
        var author = new Author { Name = model.Name.Trim() };
        db.Authors.Add(author);
        await db.SaveChangesAsync();
        TempData["Success"] = "Автор добавлен";
        return RedirectToAction(nameof(Details), new { id = author.AuthorId });
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var author = await db.Authors.AsNoTracking().FirstOrDefaultAsync(author => author.AuthorId == id);
        return author is null ? NotFound() : View(new AuthorForm { AuthorId = id, Name = author.Name });
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, AuthorForm model)
    {
        var author = await db.Authors.FindAsync(id);
        if (author is null) return NotFound();
        model.AuthorId = id;
        if (!ModelState.IsValid) return View(model);
        author.Name = model.Name.Trim();
        await db.SaveChangesAsync();
        TempData["Success"] = "Данные автора обновлены";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await db.Authors.AsNoTracking().Include(author => author.Books).FirstOrDefaultAsync(author => author.AuthorId == id);
        return author is null ? NotFound() : View(author);
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var author = await db.Authors.FindAsync(id);
        if (author is null) return NotFound();
        db.Authors.Remove(author);
        await db.SaveChangesAsync();
        TempData["Success"] = "Автор и все его книги удалены";
        return RedirectToAction(nameof(Index));
    }
}
