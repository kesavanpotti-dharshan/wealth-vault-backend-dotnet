using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WealthVaultApi.Data;
using WealthVaultApi.Models;
using WealthVaultApi.Dto;

namespace WealthVaultApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AssetsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
    {
        var assets = await _context.Assets.ToListAsync();
        var prices = new Dictionary<string, decimal> { ["bitcoin"] = 68000, ["ethereum"] = 3600, ["vti"] = 285, ["aapl"] = 180 };
        return Ok(assets);  // TODO: Add computed values in response
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Asset>> GetAsset(int id)
    {
        var asset = await _context.Assets.FindAsync(id);
        return asset is null ? NotFound() : Ok(asset);
    }

    [HttpPost]
    public async Task<ActionResult<Asset>> CreateAsset(CreateAssetDto dto)
    {
        var asset = new Asset
        {
            Type = dto.Type,
            Name = dto.Name,
            Value = dto.Value,
            YearlyYield = dto.YearlyYield,
            PurchaseDate = dto.PurchaseDate,
            PurchaseValue = dto.PurchaseValue,
            Qty = dto.Qty,
            Ticker = dto.Ticker
        };
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, asset);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsset(int id, CreateAssetDto dto)  // Reuse DTO for simplicity
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset is null) return NotFound();

        asset.Type = dto.Type;
        asset.Name = dto.Name;
        // ... map rest
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset is null) return NotFound();

        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}