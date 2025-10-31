using System.ComponentModel.DataAnnotations;
using WealthVaultApi.Models;

namespace WealthVaultApi.Dto;

public record CreateAssetDto(
    AssetType Type,
    [Required] string Name,
    decimal? Value,
    decimal? YearlyYield,
    DateOnly? PurchaseDate,
    decimal? PurchaseValue,
    decimal? Qty,
    string? Ticker
);

public record AssetResponseDto(int Id, AssetType Type, string Name, decimal? Value, decimal? YearlyYield, DateOnly? PurchaseDate, decimal? PurchaseValue, decimal? Qty, string? Ticker);