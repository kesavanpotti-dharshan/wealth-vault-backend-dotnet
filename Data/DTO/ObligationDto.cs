using System;

namespace WealthVaultApi.Data.DTO;

public record CreateObligationDto(
    int ObligationTypeId,
    string Name,
    decimal? MonthlyAmount,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null,
    string? Beneficiary = null,
    string? Notes = null,
    string? Currency = "USD"
);

public record ObligationDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public decimal? MonthlyAmount { get; init; }
    public string? Beneficiary { get; init; }
    public DateOnly? EndDate { get; init; }
}