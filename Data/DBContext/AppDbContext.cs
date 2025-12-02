using Microsoft.EntityFrameworkCore;
using WealthVaultApi.Models;

namespace WealthVaultApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<AssetTypes> AssetTypes { get; set; } = null!;
    public DbSet<Liabilities> Liabilities { get; set; } = null!;
    public DbSet<LiabilityTypes> LiabilityTypes { get; set; } = null!;
    public DbSet<Obligations> Obligations { get; set; } = null!;
    public DbSet<ObligationTypes> ObligationTypes { get; set; } = null!;

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
        modelBuilder.Entity<Liabilities>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        modelBuilder.Entity<LiabilityTypes>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        modelBuilder.Entity<Obligations>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        modelBuilder.Entity<ObligationTypes>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        
        
        // Liability Types Seed
        modelBuilder.Entity<LiabilityTypes>().HasData(
            new LiabilityTypes { Id = 1, LiabilityTypeName = "Mortgage", IconName = "home-modern" },
            new LiabilityTypes { Id = 2, LiabilityTypeName = "Student Loan", IconName = "academic-cap" },
            new LiabilityTypes { Id = 3, LiabilityTypeName = "Credit Card", IconName = "credit-card" },
            new LiabilityTypes { Id = 4, LiabilityTypeName = "Car Loan", IconName = "car" },
            new LiabilityTypes { Id = 5, LiabilityTypeName = "Personal Loan", IconName = "users" }
        );

        // Obligation Types Seed
        modelBuilder.Entity<ObligationTypes>().HasData(
            new ObligationTypes { Id = 1, ObligationTypeName = "Parents Support", IconName = "heart" },
            new ObligationTypes { Id = 2, ObligationTypeName = "Kids Education", IconName = "book-open" },
            new ObligationTypes { Id = 3, ObligationTypeName = "Charity Pledge", IconName = "hand-raised" },
            new ObligationTypes { Id = 4, ObligationTypeName = "Family Medical", IconName = "plus-circle" },
            new ObligationTypes { Id = 5, ObligationTypeName = "Studies", IconName = "plus-circle" },
            new ObligationTypes { Id = 6, ObligationTypeName = "Other", IconName = "ellipsis-h" }
        );
    }
}