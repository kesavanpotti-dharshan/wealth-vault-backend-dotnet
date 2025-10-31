using System.ComponentModel.DataAnnotations;

namespace WealthVaultApi.Models;

public enum AssetType
{
    Bank,
    Credit,
    Crypto,
    Stock
}

public class Asset
{
    public int Id { get; set; }

    [Required]
    public AssetType Type { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public decimal? Value { get; set; }
    public decimal? YearlyYield { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public decimal? PurchaseValue { get; set; }
    public decimal? Qty { get; set; }
    public string? Ticker { get; set; }

    // Computed helper (non-persisted)
    public decimal GetCurrentValue(Dictionary<string, decimal> prices) =>
        (Type == AssetType.Crypto || Type == AssetType.Stock)
            ? (Qty ?? 0) * (prices.TryGetValue(Ticker?.ToLowerInvariant() ?? string.Empty, out var p) ? p : 0)
            : (Value ?? 0);
}