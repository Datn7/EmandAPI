using AutoMapper;
using EmandAPI.Data;
using EmandAPI.Models.DTOs;
using EmandAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                return BadRequest(ModelState);

            var claim = _mapper.Map<Claim>(claimDto);
            claim.Status = "Submitted";
            claim.SubmittedAt = DateTime.UtcNow;

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Claim submitted successfully", claimId = claim.Id });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserClaims(string userId)
        {
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
