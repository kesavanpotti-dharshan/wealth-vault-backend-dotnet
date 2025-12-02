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
    public class LiabilitiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LiabilitiesController> _logger;

        public LiabilitiesController(AppDbContext context, ILogger<LiabilitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiabilityDto>>> GetLiabilities()
        {
            var liabilities = await _context.Liabilities
                .Include(l => l.LiabilityType)
                .Where(l => l.IsActive)
                .OrderBy(l => l.EndDate)
                .Select(l => new LiabilityDto
                {
                    Id = l.Id,
                    Name = l.LiabilityName,
                    Type = l.LiabilityType.LiabilityTypeName,
                    CurrentBalance = l.CurrentBalance,
                    MonthlyPayment = l.MonthlyPayment,
                    InterestRate = l.InterestRate,
                    Creditor = l.Creditor,
                    EndDate = l.EndDate,
                    IsSecured = l.IsSecured
                })
                .ToListAsync();

            return Ok(liabilities);
        }
        // GET: api/liabilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LiabilityDto>> GetLiability(int id)
        {
            var liability = await _context.Liabilities
                .Include(l => l.LiabilityType)
                .FirstOrDefaultAsync(l => l.Id == id && l.IsActive);

            if (liability == null) return NotFound();

            return Ok(new LiabilityDto
            {
                Id = liability.Id,
                Name = liability.LiabilityName,
                Type = liability.LiabilityType.LiabilityTypeName,
                CurrentBalance = liability.CurrentBalance,
                MonthlyPayment = liability.MonthlyPayment,
                InterestRate = liability.InterestRate,
                Creditor = liability.Creditor,
                EndDate = liability.EndDate,
                IsSecured = liability.IsSecured
            });
        }

        // POST: api/liabilities
        [HttpPost]
        public async Task<ActionResult<LiabilityDto>> CreateLiability([FromBody] CreateLiabilityDto dto)
        {
            var type = await _context.LiabilityTypes.FindAsync(dto.LiabilityTypeId);
            if (type == null) return BadRequest("Invalid LiabilityTypeId");

            var liability = new Liabilities
            {
                LiabilityTypeId = dto.LiabilityTypeId,
                LiabilityName = dto.Name.Trim(),
                CurrentBalance = dto.CurrentBalance,
                OriginalAmount = dto.OriginalAmount,
                InterestRate = dto.InterestRate,
                MonthlyPayment = dto.MonthlyPayment,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Creditor = dto.Creditor.Trim(),
                IsSecured = dto.IsSecured,
                Currency = dto.Currency ?? "USD",
                Notes = dto.Notes?.Trim()
            };

            _context.Liabilities.Add(liability);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLiability), new { id = liability.Id }, liability);
        }

        // PUT: api/liabilities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLiability(int id, [FromBody] UpdateLiabilityDto dto)
        {
            var liability = await _context.Liabilities.FindAsync(id);
            if (liability == null) return NotFound();

            liability.LiabilityName = dto.Name?.Trim() ?? liability.LiabilityName;
            liability.CurrentBalance = dto.CurrentBalance ?? liability.CurrentBalance;
            liability.MonthlyPayment = dto.MonthlyPayment ?? liability.MonthlyPayment;
            liability.InterestRate = dto.InterestRate ?? liability.InterestRate;
            liability.EndDate = dto.EndDate ?? liability.EndDate;
            liability.Creditor = dto.Creditor?.Trim() ?? liability.Creditor;
            liability.Notes = dto.Notes?.Trim() ?? liability.Notes;
            liability.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/liabilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiability(int id)
        {
            var liability = await _context.Liabilities.FindAsync(id);
            if (liability == null) return NotFound();

            liability.IsActive = false;
            liability.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
