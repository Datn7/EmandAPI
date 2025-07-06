using AutoMapper;
using EmandAPI.Data;
using EmandAPI.Models.DTOs;
using EmandAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Emand_Medical_Insurance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PoliciesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PoliciesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserPolicies()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var policies = await _context.Policies
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var policyDtos = _mapper.Map<List<PolicyDTO>>(policies);
            return Ok(policyDtos);
        }



        [HttpPost]
        public async Task<IActionResult> CreatePolicy([FromBody] PolicyDTO policyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var policy = _mapper.Map<Policy>(policyDto);
            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Policy created successfully", policyId = policy.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] PolicyDTO policyDto)
        {
            if (id != policyDto.Id)
                return BadRequest("ID mismatch.");

            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
                return NotFound();

            _mapper.Map(policyDto, policy);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Policy updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
                return NotFound();

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Policy deleted successfully" });
        }
    }
}
