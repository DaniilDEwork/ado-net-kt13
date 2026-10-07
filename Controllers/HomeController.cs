using Kt13Library.Data;
using Kt13Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kt13Library.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(new HomeModel
        {
            AuthorsCount = await db.Authors.CountAsync(),
            BooksCount = await db.Books.CountAsync()
        });
    }

    public IActionResult Error()
    {
        Response.StatusCode = 500;
        return View();
    }
}
