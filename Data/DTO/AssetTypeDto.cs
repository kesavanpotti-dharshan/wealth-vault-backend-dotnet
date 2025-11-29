using System;

namespace WealthVaultApi.Data.DTO;

public record AssetTypeDto
{
    public string AssetName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int RiskLevel { get; set; }
    public decimal DefaultYield { get; set; }
    public bool TaxAdvantaged { get; set; } = false;
    public bool IsLiability { get; set; } = false;
    public bool IsLiquid { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? ModifiedDate { get; set; }

}
