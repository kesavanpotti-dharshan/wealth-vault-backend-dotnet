using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WealthVaultApi.Models;

public enum IncomeFrequency
{
    Monthly = 1,
    Quarterly = 3,
    SemiAnnually = 6,
    Annually = 12,
    OneTime = 999
}

public class Asset
{
    // Core Identity
    public int Id { get; set; }

    // Foreign Key to your AssetTypes table (flexible categories)
    public int AssetTypeId { get; set; }
    public AssetTypes AssetType { get; set; } = null!;

    // Human-readable name
    [Required, MaxLength(100)]
    public string AssetName { get; set; } = string.Empty;

    // Optional: Ticker/symbol for stocks/crypto (e.g., AAPL, BTC)
    public string? Ticker { get; set; }

    // Current total market value (auto-calculated for qty-based assets)
    public decimal? CurrentValue { get; set; } = 0;

    // Quantity owned (for stocks, crypto, real estate shares, etc.)
    public decimal? Quantity { get; set; }

    // Purchase price per unit (for ROI calculations)
    public decimal? PurchasePricePerUnit { get; set; }

    // Total cost basis (Quantity × PurchasePrice or manual entry)
    public decimal? CostBasis { get; set; }

    // Income Generation — The Heart of Your App
    public decimal? AnnualIncome { get; set; } = 0;           // e.g., $5,200 rent, $1,200 dividends
    public decimal? YieldPercentage { get; set; } = 0;        // e.g., 6.2% rental yield
    public IncomeFrequency IncomeFrequency { get; set; } = IncomeFrequency.Annually;

    // Dates
    public DateOnly? PurchaseDate { get; set; }
    public DateOnly? LastIncomeDate { get; set; }             // Track last dividend/rent received
    public DateOnly? NextIncomeDate { get; set; }             // Forecast next payout
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    // Currency & Location
    public string Currency { get; set; } = "USD";
    public string? Country { get; set; }                      // For tax, real estate
    public string? SecondaryCurrency { get; set; } = "INR";  // e.g., local currency for foreign assets

    // Optional Notes & Metadata
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;

    // Computed (not stored) — use in API responses
    [NotMapped]
    public decimal? UnrealizedGainLoss => CurrentValue - (CostBasis ?? 0);
    
    [NotMapped]
    public decimal? UnrealizedGainLossPercent => 
        CostBasis > 0 ? (CurrentValue - CostBasis) / CostBasis * 100 : null;

    [NotMapped]
    public decimal? MonthlyIncome => AnnualIncome / 
        (IncomeFrequency == IncomeFrequency.Monthly ? 1 : 
         IncomeFrequency == IncomeFrequency.Quarterly ? 3 : 
         IncomeFrequency == IncomeFrequency.SemiAnnually ? 6 : 12);
}