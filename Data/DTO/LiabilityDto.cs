using System;

namespace WealthVaultApi.Data.DTO;

public record CreateLiabilityDto(
    int LiabilityTypeId,
    string Name,
    decimal CurrentBalance,
    decimal OriginalAmount,
    decimal InterestRate,
    decimal MonthlyPayment,
    DateOnly StartDate,
    DateOnly? EndDate,
    string Creditor,
    bool IsSecured,
    string? Description = null,
    string? Currency = "USD",
    string? Notes = null
);

public record UpdateLiabilityDto(
    string? Name = null,
    decimal? CurrentBalance = null,
    decimal? MonthlyPayment = null,
    decimal? InterestRate = null,
    DateOnly? EndDate = null,
    string? Creditor = null,
    string? Notes = null
);

public record LiabilityDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public decimal CurrentBalance { get; init; }
    public decimal MonthlyPayment { get; init; }
    public decimal InterestRate { get; init; }
    public string Creditor { get; init; } = string.Empty;
    public DateOnly? EndDate { get; init; }
    public bool IsSecured { get; init; }
}
