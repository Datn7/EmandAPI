using AutoMapper;
using EmandAPI.Data;
using EmandAPI.Models.DTOs;
using EmandAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmandAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClaimsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ClaimsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim([FromBody] ClaimDTO claimDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors });
            }

            // Check policy existence and ownership:
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.Id == claimDto.PolicyId && p.UserId == userId);
            if (policy == null)
            {
                return BadRequest(new { message = "Invalid PolicyId: Policy not found or not owned by user." });
            }

            var claim = _mapper.Map<Models.Entities.Claim>(claimDto);
            claim.Status = "Submitted";
            claim.SubmittedAt = DateTime.UtcNow;

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Claim submitted successfully", claimId = claim.Id });
        }


        [HttpGet("user")]
        public async Task<IActionResult> GetUserClaims()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId not found in token.");

            var claims = await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.Policy.UserId == userId)
                .ToListAsync();

            var claimDtos = _mapper.Map<List<ClaimDTO>>(claims);

            return Ok(claimDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaimById(int id)
        {
            var claim = await _context.Claims
                .Include(c => c.Policy)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (claim == null)
                return NotFound();

            var claimDto = _mapper.Map<ClaimDTO>(claim);

            return Ok(claimDto);
        }
    }
}
