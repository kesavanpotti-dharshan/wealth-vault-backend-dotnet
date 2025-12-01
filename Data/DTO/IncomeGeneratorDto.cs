using System;

namespace WealthVaultApi.Data.DTO;

public class IncomeGeneratorDto
{
    public int Rank { get; set; }
    public int AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public decimal AnnualIncome { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal PercentageOfTotal { get; set; }
}
