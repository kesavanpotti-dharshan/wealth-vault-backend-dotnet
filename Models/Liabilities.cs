using System;
using System.ComponentModel.DataAnnotations;

namespace WealthVaultApi.Models;

public class Liabilities
{
    public int Id { get; set; }

    public int LiabilityTypeId { get; set; }
    public LiabilityTypes LiabilityType { get; set; } = null!;

    [Required, MaxLength(100)]
    public string LiabilityName { get; set; } = string.Empty;           // "30-Year Mortgage"

    public decimal CurrentBalance { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal InterestRate { get; set; }                  // 4.25%
    public decimal MonthlyPayment { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }                     // Final payoff
    public string Creditor { get; set; } = string.Empty;       // "Bank of America"
    public bool IsSecured { get; set; } = true;

    public string Currency { get; set; } = "USD";
    public string? Notes { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
