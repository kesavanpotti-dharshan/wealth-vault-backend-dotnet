using Microsoft.EntityFrameworkCore;
using WealthVaultApi.Models;

namespace WealthVaultApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<AssetTypes> AssetTypes { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        modelBuilder.Entity<AssetTypes>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}