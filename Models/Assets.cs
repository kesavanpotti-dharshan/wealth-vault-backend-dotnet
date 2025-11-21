using System.ComponentModel.DataAnnotations;

namespace WealthVaultApi.Models;

public enum AssetType
{
    Bank,
    Credit,
    Crypto,
    Stock
}

public enum AssetCategory
{
    Cash,
    TaxAdvantagedInvestment,
    TaxDeferredInvestment,
    TaxableInvestment,
    Obligations,
    Liabilities
}

public class Asset
{
    public int Id { get; set; }
    [Required]
    public AssetType AssetType { get; set; }
    public AssetCategory AssetCategory { get; set; } = AssetCategory.Cash;
    [Required, MaxLength(100)]
    public string AssetName { get; set; } = string.Empty;
    public decimal? AssetTotalValue { get; set; } = 0;
    public decimal? AssetYield { get; set; } = 0;
    public DateOnly? LastUpdatedDate { get; set; } 
    public string? AssetCurrency { get; set; } = "USD";
    
}