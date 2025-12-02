using System;
using System.ComponentModel.DataAnnotations;

namespace WealthVaultApi.Models;

public class LiabilityTypes
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string LiabilityTypeName { get; set; } = string.Empty;
    public string IconName { get; set; } = "credit-card";
    public bool IsActive { get; set; } = true;
    public ICollection<Liabilities> Liabilities { get; set; } = new List<Liabilities>();
}
