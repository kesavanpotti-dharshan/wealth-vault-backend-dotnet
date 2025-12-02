using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WealthVaultApi.Data;
using WealthVaultApi.Data.DTO;
using WealthVaultApi.Models;

namespace WealthVaultApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObligationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ObligationsController> _logger;

        public ObligationsController(AppDbContext context, ILogger<ObligationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObligationDto>>> GetObligations()
        {
            var obligations = await _context.Obligations
                .Include(o => o.ObligationType)
                .Where(o => o.IsActive)
                .OrderBy(o => o.EndDate)
                .Select(o => new ObligationDto
                {
                    Id = o.Id,
                    Name = o.ObligationName,
                    Type = o.ObligationType.ObligationTypeName,
                    MonthlyAmount = o.MonthlyAmount,
                    Beneficiary = o.Beneficiary,
                    EndDate = o.EndDate
                })
                .ToListAsync();

            return Ok(obligations);
        }
        [HttpPost]
        public async Task<ActionResult<Obligations>> CreateObligation([FromBody] CreateObligationDto dto)
        {
            var type = await _context.ObligationTypes.FindAsync(dto.ObligationTypeId);
            if (type == null) return BadRequest("Invalid ObligationTypeId");

            var obligation = new Obligations
            {
                ObligationTypeId = dto.ObligationTypeId,
                ObligationName = dto.Name.Trim(),
                MonthlyAmount = (decimal)dto.MonthlyAmount,
                AnnualAmount = dto.MonthlyAmount * 12,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Beneficiary = dto.Beneficiary?.Trim(),
                Notes = dto.Notes?.Trim(),
                Currency = dto.Currency ?? "USD"
            };

            _context.Obligations.Add(obligation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetObligation", new { id = obligation.Id }, obligation);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ObligationDto>> GetObligation(int id)
        {
            var o = await _context.Obligations
                .Include(o => o.ObligationType)
                .FirstOrDefaultAsync(o => o.Id == id && o.IsActive);

            if (o == null) return NotFound();

            return new ObligationDto
            {
                Id = o.Id,
                Name = o.ObligationName,
                Type = o.ObligationType.ObligationTypeName,
                MonthlyAmount = o.MonthlyAmount,
                Beneficiary = o.Beneficiary,
                EndDate = o.EndDate
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObligation(int id, CreateObligationDto dto)
        {
            var o = await _context.Obligations.FindAsync(id);
            if (o == null) return NotFound();

            o.ObligationName = dto.Name?.Trim() ?? o.ObligationName;
            o.MonthlyAmount = dto.MonthlyAmount ?? o.MonthlyAmount;
            o.AnnualAmount = (dto.MonthlyAmount ?? o.MonthlyAmount) * 12;
            o.EndDate = dto.EndDate ?? o.EndDate;
            o.Beneficiary = dto.Beneficiary?.Trim() ?? o.Beneficiary;
            o.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObligation(int id)
        {
            var o = await _context.Obligations.FindAsync(id);
            if (o == null) return NotFound();

            o.IsActive = false;
            o.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}