using Microsoft.EntityFrameworkCore;
using WealthVaultApi.Models;

namespace WealthVaultApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Asset> Assets { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasConversion<string>();
            entity.HasData(  // Seed samples – convert your JS ones
                new Asset { Id = 1, Type = AssetType.Bank, Name = "Chase Savings", Value = 75000, YearlyYield = 1200 },
                new Asset { Id = 2, Type = AssetType.Crypto, Name = "Bitcoin", Ticker = "bitcoin", Qty = 1, PurchaseDate = DateOnly.FromDateTime(new DateTime(2025,1,20)), PurchaseValue = 12000 }
            );
        });
    }
}