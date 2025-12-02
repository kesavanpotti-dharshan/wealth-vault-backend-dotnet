using System;
using System.ComponentModel.DataAnnotations;

namespace WealthVaultApi.Models;

public class ObligationTypes
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string ObligationTypeName { get; set; } = string.Empty;
    public string IconName { get; set; } = "heart";
    public bool IsActive { get; set; } = true;
    public ICollection<Obligations> Obligations { get; set; } = new List<Obligations>();
}
