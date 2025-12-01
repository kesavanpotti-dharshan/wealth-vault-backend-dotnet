using System.ComponentModel.DataAnnotations;
using WealthVaultApi.Models;

namespace WealthVaultApi.Dto;

public record CreateAssetDto(
    int AssetTypeId,
    string Name,
    string? Ticker = null,

    // Value
    decimal? CurrentValue = null,
    decimal? Quantity = null,
    decimal? PurchasePricePerUnit = null,
    decimal? CostBasis = null,

    // Income
    decimal? AnnualIncome = null,
    decimal? YieldPercentage = null,
    IncomeFrequency? IncomeFrequency = null,

    // Dates
    DateOnly? PurchaseDate = null,
    DateOnly? LastIncomeDate = null,
    DateOnly? NextIncomeDate = null,

    // Optional
    string? Currency = "USD",
    string? Country = null,
    string? Notes = null
);

public record AssetResponseDto
(
int Id,
string AssetType,
string AssetCategory,
string AssetName = "",
decimal? AssetTotalValue = 0,
decimal? AssetYield = 0,
DateOnly? LastUpdatedDate = null,
string? AssetCurrency = "USD"
);