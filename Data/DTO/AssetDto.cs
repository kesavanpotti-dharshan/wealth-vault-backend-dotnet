using System.ComponentModel.DataAnnotations;
using WealthVaultApi.Models;

namespace WealthVaultApi.Dto;

public record CreateAssetDto(
    AssetType AssetType,
    AssetCategory AssetCategory,
    string AssetName = "",
    decimal? AssetTotalValue = 0,
    decimal? AssetYield = 0,
    DateOnly? LastUpdatedDate = null,
    string? AssetCurrency = "USD"
);

public record AssetResponseDto
(
int Id,
AssetType AssetType,
AssetCategory AssetCategory,
string AssetName = "",
decimal? AssetTotalValue = 0,
decimal? AssetYield = 0,
DateOnly? LastUpdatedDate = null,
string? AssetCurrency = "USD"
);