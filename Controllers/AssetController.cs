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
    private readonly ILogger<AssetsController> _logger;

    public AssetsController(AppDbContext context, ILogger<AssetsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
    {
        _logger.LogInformation("Fetching all assets");
        var assets = await _context.Assets.ToListAsync();
        var prices = new Dictionary<string, decimal> { ["bitcoin"] = 68000, ["ethereum"] = 3600, ["vti"] = 285, ["aapl"] = 180 };
        _logger.LogDebug("Retrieved {Count} assets", assets.Count);
        return Ok(assets);  // TODO: Add computed values in response
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Asset>> GetAsset(int id)
    {
        _logger.LogInformation("Fetching asset with id {AssetId}", id);
        var asset = await _context.Assets.FindAsync(id);
        if (asset is null)
        {
            _logger.LogWarning("Asset with id {AssetId} not found", id);
            return NotFound();
        }
        _logger.LogDebug("Asset retrieved: {AssetName}", asset.AssetName);
        return Ok(asset);
    }

    [HttpPost]
    public async Task<ActionResult<Asset>> CreateAsset(CreateAssetDto dto)
    {
        _logger.LogInformation("Creating new asset: {AssetName}", dto.AssetName);
        var asset = new Asset
        {
            AssetType = dto.AssetType,
            AssetCategory = dto.AssetCategory,
            AssetName = dto.AssetName,
            AssetTotalValue = dto.AssetTotalValue,
            AssetYield = dto.AssetYield,
            LastUpdatedDate = dto.LastUpdatedDate,
            AssetCurrency = dto.AssetCurrency
        };
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Asset created successfully with id {AssetId}", asset.Id);
        return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, asset);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsset(int id, CreateAssetDto dto)  // Reuse DTO for simplicity
    {
        _logger.LogInformation("Updating asset with id {AssetId}", id);
        var asset = await _context.Assets.FindAsync(id);
        if (asset is null)
        {
            _logger.LogWarning("Asset with id {AssetId} not found for update", id);
            return NotFound();
        }

        asset.AssetType = dto.AssetType;
        asset.AssetCategory = dto.AssetCategory;
        asset.AssetName = dto.AssetName;
        asset.AssetTotalValue = dto.AssetTotalValue;
        asset.AssetYield = dto.AssetYield;
        asset.LastUpdatedDate = dto.LastUpdatedDate;
        asset.AssetCurrency = dto.AssetCurrency;
        
        await _context.SaveChangesAsync();
        _logger.LogInformation("Asset with id {AssetId} updated successfully", id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        _logger.LogInformation("Deleting asset with id {AssetId}", id);
        var asset = await _context.Assets.FindAsync(id);
        if (asset is null)
        {
            _logger.LogWarning("Asset with id {AssetId} not found for deletion", id);
            return NotFound();
        }

        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Asset with id {AssetId} deleted successfully", id);
        return NoContent();
    }
}