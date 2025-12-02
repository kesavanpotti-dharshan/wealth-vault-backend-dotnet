using System;
using System.ComponentModel.DataAnnotations;

namespace WealthVaultApi.Models;

public class Obligations
{
    public int Id { get; set; }

    public int ObligationTypeId { get; set; }
    public ObligationTypes ObligationType { get; set; } = null!;

    [Required, MaxLength(100)]
    public string ObligationName { get; set; } = string.Empty;           // "Parents Monthly Support"

    public decimal MonthlyAmount { get; set; }
    public decimal? AnnualAmount { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }                     // e.g., until kid graduates

    public string? Beneficiary { get; set; }                   // "Mom & Dad", "Harvard Fund"
    public string? Notes { get; set; }

    public string Currency { get; set; } = "USD";
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
