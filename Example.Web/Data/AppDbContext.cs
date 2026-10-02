using Example.Web.Models;
using Microsoft.EntityFrameworkCore;
namespace Example.Web.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ProductCodeItem> ProductCodeItems => Set<ProductCodeItem>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var item = modelBuilder.Entity<ProductCodeItem>();
        item.Property(x => x.Code).HasMaxLength(35).IsUnicode(false).IsRequired();
        item.HasIndex(x => x.Code).IsUnique();
        item.Property(x => x.CreatedAt).HasColumnType("datetime2");
    }
}

