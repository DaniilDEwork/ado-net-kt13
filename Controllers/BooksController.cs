using Kt13Library.Data;
using Kt13Library.Models;
using Kt13Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kt13Library.Controllers;

[Route("Books")]
public class BooksController(AppDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await db.Books.AsNoTracking().Include(book => book.Author).OrderBy(book => book.BookId).ToListAsync());
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var book = await db.Books.AsNoTracking().Include(book => book.Author).FirstOrDefaultAsync(book => book.BookId == id);
        return book is null ? NotFound() : View(book);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create(int? authorId)
    {
        var model = new BookForm { AuthorId = authorId };
        await LoadAuthorsAsync(model);
        return View(model);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(BookForm model)
    {
        await ValidateAuthorAsync(model.AuthorId);
        if (!ModelState.IsValid)
        {
            await LoadAuthorsAsync(model);
            return View(model);
        }
        var book = new Book { Title = model.Title.Trim(), AuthorId = model.AuthorId!.Value };
        db.Books.Add(book);
        await db.SaveChangesAsync();
        TempData["Success"] = "Книга добавлена";
        return RedirectToAction(nameof(Details), new { id = book.BookId });
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await db.Books.AsNoTracking().FirstOrDefaultAsync(book => book.BookId == id);
        if (book is null) return NotFound();
        var model = new BookForm { BookId = id, Title = book.Title, AuthorId = book.AuthorId };
        await LoadAuthorsAsync(model);
        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, BookForm model)
    {
        var book = await db.Books.FindAsync(id);
        if (book is null) return NotFound();
        model.BookId = id;
        await ValidateAuthorAsync(model.AuthorId);
        if (!ModelState.IsValid)
        {
            await LoadAuthorsAsync(model);
            return View(model);
        }
        book.Title = model.Title.Trim();
        book.AuthorId = model.AuthorId!.Value;
        await db.SaveChangesAsync();
        TempData["Success"] = "Данные книги обновлены";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await db.Books.AsNoTracking().Include(book => book.Author).FirstOrDefaultAsync(book => book.BookId == id);
        return book is null ? NotFound() : View(book);
    }

    [HttpPost("Delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await db.Books.FindAsync(id);
        if (book is null) return NotFound();
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        TempData["Success"] = "Книга удалена";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateAuthorAsync(int? authorId)
    {
        if (authorId.HasValue && !await db.Authors.AnyAsync(author => author.AuthorId == authorId.Value))
        {
            ModelState.AddModelError(nameof(BookForm.AuthorId), "Выбранный автор не найден");
        }
    }

    private async Task LoadAuthorsAsync(BookForm model)
    {
        model.Authors = await db.Authors.AsNoTracking().OrderBy(author => author.Name)
            .Select(author => new SelectListItem { Value = author.AuthorId.ToString(), Text = author.Name }).ToListAsync();
    }
}
