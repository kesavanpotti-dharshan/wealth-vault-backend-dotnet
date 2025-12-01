using System;

namespace WealthVaultApi.Data.DTO;

public class AssetSummaryDto
{
    public decimal TotalNetWorth { get; set; }
    public decimal MonthlyPassiveIncome { get; set; }
    public decimal TotalAnnualIncome { get; set; }
    public decimal PortfolioYield { get; set; }
    public int AssetCount { get; set; }
    public List<IncomeGeneratorDto> TopIncomeGenerators { get; set; } = new();
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
