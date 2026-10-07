using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kt13Library.ViewModels;

public class AuthorForm
{
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "Введите имя автора")]
    [StringLength(100, ErrorMessage = "Имя не должно быть длиннее 100 символов")]
    public string Name { get; set; } = "";
}

public class BookForm
{
    public int BookId { get; set; }

    [Required(ErrorMessage = "Введите название книги")]
    [StringLength(200, ErrorMessage = "Название не должно быть длиннее 200 символов")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Выберите автора")]
    public int? AuthorId { get; set; }

    [ValidateNever]
    public List<SelectListItem> Authors { get; set; } = new();
}

public class HomeModel
{
    public int AuthorsCount { get; set; }
    public int BooksCount { get; set; }
}
