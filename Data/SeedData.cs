using Kt13Library.Models;

namespace Kt13Library.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (!await db.Database.EnsureCreatedAsync()) return;
        db.Authors.AddRange(
            new Author
            {
                Name = "Александр Пушкин",
                Books = new List<Book>
                {
                    new() { Title = "Евгений Онегин" },
                    new() { Title = "Капитанская дочка" }
                }
            },
            new Author
            {
                Name = "Лев Толстой",
                Books = new List<Book> { new() { Title = "Война и мир" } }
            });
        await db.SaveChangesAsync();
    }
}
