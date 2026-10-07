using Kt13Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Kt13Library.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Author>().HasKey(author => author.AuthorId);
        builder.Entity<Author>().Property(author => author.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Book>().HasKey(book => book.BookId);
        builder.Entity<Book>().Property(book => book.Title).IsRequired().HasMaxLength(200);
        builder.Entity<Author>()
            .HasMany(author => author.Books)
            .WithOne(book => book.Author)
            .HasForeignKey(book => book.AuthorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
