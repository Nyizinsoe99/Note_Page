using Microsoft.EntityFrameworkCore;

namespace Node_Page.Model;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Indexes improve search performance in OData
        modelBuilder.Entity<Note>().HasIndex(n => n.Title);
    }
}