using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WealthVaultApi.Data;
using WealthVaultApi.Models;
using WealthVaultApi.Dto;
using WealthVaultApi.Data.DTO;

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
        _logger.LogInformation("Creating new asset: {AssetName}", dto.Name);

        // Validate that the AssetType exists and is active
        var assetType = await _context.AssetTypes
            .FirstOrDefaultAsync(t => t.Id == dto.AssetTypeId && t.IsActive);

        if (assetType == null)
            return BadRequest(new { error = "Invalid or inactive AssetTypeId" });

        // Optional: Prevent liabilities from having positive yield (business rule)
        if (assetType.IsLiability && dto.AnnualIncome > 0)
            return BadRequest(new { error = "Liabilities cannot generate positive income" });

        var asset = new Asset
        {
            AssetTypeId = dto.AssetTypeId,
            AssetType = assetType, // Navigation property (optional but nice for response)

            AssetName = dto.Name.Trim(),
            Ticker = dto.Ticker?.Trim().ToUpperInvariant(),

            // Value handling — smart logic
            CurrentValue = dto.CurrentValue ?? 0m,
            Quantity = dto.Quantity,
            PurchasePricePerUnit = dto.PurchasePricePerUnit,
            CostBasis = dto.CostBasis ?? (dto.Quantity.HasValue && dto.PurchasePricePerUnit.HasValue
            ? dto.Quantity.Value * dto.PurchasePricePerUnit.Value
            : null),

            // Income — the heart of Arca Nostra
            AnnualIncome = dto.AnnualIncome ?? 0m,
            YieldPercentage = dto.YieldPercentage,
            IncomeFrequency = dto.IncomeFrequency ?? IncomeFrequency.Annually,

            // Dates
            PurchaseDate = dto.PurchaseDate,
            LastIncomeDate = dto.LastIncomeDate,
            NextIncomeDate = dto.NextIncomeDate,
            LastUpdated = DateTime.UtcNow,

            // Metadata
            Currency = dto.Currency ?? "USD",
            Country = dto.Country,
            Notes = dto.Notes?.Trim(),
            IsActive = true
        };
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Asset created successfully with id {AssetId}", asset.Id);
        return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, asset);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsset(int id, [FromBody] CreateAssetDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("Updating asset with id {AssetId}", id);

        var asset = await _context.Assets
            .Include(a => a.AssetType)
            .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);

        if (asset is null)
        {
            _logger.LogWarning("Asset with id {AssetId} not found or inactive", id);
            return NotFound(new { error = "Asset not found or has been deactivated" });
        }

        // Validate AssetType exists and is active
        var newAssetType = await _context.AssetTypes
            .FirstOrDefaultAsync(t => t.Id == dto.AssetTypeId && t.IsActive);

        if (newAssetType == null)
            return BadRequest(new { error = "Invalid or inactive AssetTypeId" });

        // Business rule: Liabilities can't generate positive income
        if (newAssetType.IsLiability && (dto.AnnualIncome > 0 || (dto.YieldPercentage.HasValue && dto.YieldPercentage > 0)))
            return BadRequest(new { error = "Liabilities cannot generate positive income" });

        // Update all fields
        asset.AssetTypeId = dto.AssetTypeId;
        asset.AssetType = newAssetType;

        asset.AssetName = dto.Name.Trim();
        asset.Ticker = dto.Ticker?.Trim().ToUpperInvariant();

        // Value updates
        asset.CurrentValue = dto.CurrentValue ?? asset.CurrentValue;
        asset.Quantity = dto.Quantity ?? asset.Quantity;
        asset.PurchasePricePerUnit = dto.PurchasePricePerUnit ?? asset.PurchasePricePerUnit;

        // Smart CostBasis recalculation (only if inputs provided)
        if (dto.Quantity.HasValue || dto.PurchasePricePerUnit.HasValue)
        {
            var qty = dto.Quantity ?? asset.Quantity ?? 0m;
            var price = dto.PurchasePricePerUnit ?? asset.PurchasePricePerUnit ?? 0m;
            asset.CostBasis = qty * price;
        }
        else if (dto.CostBasis.HasValue)
        {
            asset.CostBasis = dto.CostBasis.Value;
        }

        // Income updates
        asset.AnnualIncome = dto.AnnualIncome ?? asset.AnnualIncome ?? 0m;
        asset.YieldPercentage = dto.YieldPercentage ?? asset.YieldPercentage;
        asset.IncomeFrequency = dto.IncomeFrequency ?? asset.IncomeFrequency;

        // Dates
        asset.PurchaseDate = dto.PurchaseDate ?? asset.PurchaseDate;
        asset.LastIncomeDate = dto.LastIncomeDate ?? asset.LastIncomeDate;
        asset.NextIncomeDate = dto.NextIncomeDate ?? asset.NextIncomeDate;

        // Metadata
        asset.Currency = dto.Currency ?? asset.Currency;
        asset.Country = dto.Country ?? asset.Country;
        asset.Notes = dto.Notes?.Trim() ?? asset.Notes;

        // Always update timestamp
        asset.LastUpdated = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Asset {AssetId} updated successfully", id);
            return NoContent(); // 204 — standard for successful PUT
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error updating asset {AssetId}", id);
            return StatusCode(500, new { error = "Failed to update asset" });
        }
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
    [HttpGet("GetAssetTypes")]
    public async Task<ActionResult<IEnumerable<AssetTypes>>> GetAssetTypes()
    {
        _logger.LogInformation("Fetching all asset types");
        var assetTypes = await _context.AssetTypes.ToListAsync();
        _logger.LogDebug("Retrieved {Count} asset types", assetTypes.Count);
        return Ok(assetTypes);
    }
    [HttpPost("CreateAssetType")]
    public async Task<ActionResult<AssetTypes>> CreateAssetType(AssetTypeDto dto)
    {
        _logger.LogInformation("Creating new asset: {AssetName}", dto.AssetName);
        var assetType = new AssetTypes
        {
            AssetName = dto.AssetName,
            Description = dto.Description,
            RiskLevel = dto.RiskLevel,
            DefaultYield = dto.DefaultYield,
            TaxAdvantaged = dto.TaxAdvantaged,
            IsLiability = dto.IsLiability,
            IsLiquid = dto.IsLiquid,
            IsActive = dto.IsActive,
            CreatedDate = dto.CreatedDate,
            ModifiedDate = dto.ModifiedDate
        };
        _context.AssetTypes.Add(assetType);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Asset type created successfully with id {AssetTypeId}", assetType.Id);
        return CreatedAtAction(nameof(GetAssetTypes), new { id = assetType.Id }, assetType);
    }
    [HttpGet("summary")]
    public async Task<ActionResult<AssetSummaryDto>> GetAssetSummary()
    {
        _logger.LogInformation("Generating asset summary report");
        var assets = await _context.Assets
            .Include(a => a.AssetType)
            .Where(a => a.IsActive) // only active assets
            .ToListAsync();

        // Mock real-time prices (replace with real API later)
        var priceCache = new Dictionary<string, decimal>
        {
            ["bitcoin"] = 108900m,
            ["ethereum"] = 4920m,
            ["vti"] = 301m,
            ["aapl"] = 238m
        };

        decimal totalNetWorth = 0m;
        decimal totalAnnualIncome = 0m;
        var incomeGenerators = new List<IncomeGeneratorDto>();

        foreach (var asset in assets)
        {
            _logger.LogInformation("Processing asset {AssetName} (ID: {AssetId})", asset.AssetName, asset.Id);            
            
            decimal computedValue = 0m;
            var tickerKey = asset.Ticker?.ToLower() ?? string.Empty;
            _logger.LogInformation("Calculating value for asset with ticker {Ticker}", tickerKey);
            _logger.LogInformation("Asset details: Quantity={Quantity}, CurrentValue={CurrentValue}, AssetType={AssetType}",
                asset.Quantity, asset.CurrentValue, asset.AssetType?.AssetName ?? "Not Available");

            computedValue = asset.AssetType switch
            {
                // For stocks/crypto, use quantity * current market price
                AssetTypes at when at.AssetName.Equals("Stock", StringComparison.OrdinalIgnoreCase) ||
                                 at.AssetName.Equals("Cryptocurrency", StringComparison.OrdinalIgnoreCase) =>
                    (asset.Quantity ?? 0m) * priceCache.GetValueOrDefault(tickerKey, asset.CurrentValue ?? 0m),

                // For cash or fixed value assets, use current value directly
                AssetTypes at when at.AssetName.Equals("Cash", StringComparison.OrdinalIgnoreCase) ||
                                 at.AssetName.Equals("Bond", StringComparison.OrdinalIgnoreCase) =>
                    asset.CurrentValue ?? 0m,
                
                AssetTypes at when at.AssetName.Equals("High-Yield Savings", StringComparison.OrdinalIgnoreCase) =>
                    (asset.Quantity ?? 0m) * (asset.CurrentValue ?? 0m) * ((asset.YieldPercentage ?? 0m) / 100m),

                // Default fallback
                _ => asset.CurrentValue ?? 0m,
            };

            _logger.LogInformation("Computed value for asset {AssetName} is {ComputedValue}", asset.AssetName, computedValue);

            // Apply sign for liabilities
            decimal currentValue = asset.AssetType?.IsLiability == true ? -computedValue : computedValue;

            _logger.LogInformation("{Quantity} units of {Ticker} valued at {UnitPrice} each gives current value {CurrentValue}",
                asset.Quantity, asset.Ticker, priceCache.GetValueOrDefault(tickerKey, asset.CurrentValue ?? 0m), currentValue);

            totalNetWorth += currentValue;

            // Income calculation
            decimal annualIncome = asset.AnnualIncome ??
                (asset.CurrentValue ?? 0m) * (asset.YieldPercentage ?? 0m) / 100m;

            if (annualIncome > 0)
            {
                decimal monthlyIncome = annualIncome / 12m;

                totalAnnualIncome += annualIncome;

                incomeGenerators.Add(new IncomeGeneratorDto
                {
                    AssetId = asset.Id,
                    AssetName = asset.AssetName,
                    AssetType = asset.AssetType?.AssetName ?? "Unknown",
                    AnnualIncome = annualIncome,
                    MonthlyIncome = monthlyIncome
                });
            }
        }

        // Final calculations
        decimal monthlyPassiveIncome = totalAnnualIncome / 12m;
        decimal portfolioYield = totalNetWorth > 0
            ? (totalAnnualIncome / totalNetWorth) * 100m
            : 0m;

        var topGenerators = incomeGenerators
            .OrderByDescending(x => x.AnnualIncome)
            .Take(5)
            .Select((g, i) => new IncomeGeneratorDto
            {
                Rank = i + 1,
                AssetId = g.AssetId,
                AssetName = g.AssetName,
                AssetType = g.AssetType,
                AnnualIncome = g.AnnualIncome,
                MonthlyIncome = g.MonthlyIncome,
                PercentageOfTotal = totalAnnualIncome > 0 ? (g.AnnualIncome / totalAnnualIncome) * 100m : 0m
            })
            .ToList();

        var response = new AssetSummaryDto
        {
            TotalNetWorth = Math.Round(totalNetWorth, 2),
            MonthlyPassiveIncome = Math.Round(monthlyPassiveIncome, 2),
            TotalAnnualIncome = Math.Round(totalAnnualIncome, 2),
            PortfolioYield = Math.Round(portfolioYield, 2),
            AssetCount = assets.Count,
            TopIncomeGenerators = topGenerators,
            GeneratedAt = DateTime.UtcNow
        };

        return Ok(response);
    }
}