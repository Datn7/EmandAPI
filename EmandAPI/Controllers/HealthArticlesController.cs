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
    [Authorize(Roles = "Admin")]
    public class HealthArticlesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HealthArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthArticleDTO>>> GetHealthArticles()
        {
            var articles = await _context.HealthArticles
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new HealthArticleDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Content = a.Content,
                    Category = a.Category,
                    Tags = a.Tags,
                    ImageUrl = a.ImageUrl,
                    ReadTimeMinutes = a.ReadTimeMinutes,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HealthArticleDTO>> GetHealthArticle(int id)
        {
            var a = await _context.HealthArticles.FindAsync(id);
            if (a == null) return NotFound();

            var dto = new HealthArticleDTO
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Content = a.Content,
                Category = a.Category,
                Tags = a.Tags,
                ImageUrl = a.ImageUrl,
                ReadTimeMinutes = a.ReadTimeMinutes,
                CreatedAt = a.CreatedAt
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateHealthArticle(CreateHealthArticleDTO dto)
        {
            var article = new HealthArticle
            {
                Title = dto.Title,
                Description = dto.Description,
                Content = dto.Content,
                Category = dto.Category,
                Tags = dto.Tags,
                ImageUrl = dto.ImageUrl,
                ReadTimeMinutes = dto.ReadTimeMinutes,
                CreatedAt = DateTime.UtcNow
            };

            _context.HealthArticles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHealthArticle), new { id = article.Id }, article);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHealthArticle(int id, CreateHealthArticleDTO dto)
        {
            var article = await _context.HealthArticles.FindAsync(id);
            if (article == null) return NotFound();

            article.Title = dto.Title;
            article.Description = dto.Description;
            article.Content = dto.Content;
            article.Category = dto.Category;
            article.Tags = dto.Tags;
            article.ImageUrl = dto.ImageUrl;
            article.ReadTimeMinutes = dto.ReadTimeMinutes;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHealthArticle(int id)
        {
            var article = await _context.HealthArticles.FindAsync(id);
            if (article == null) return NotFound();

            _context.HealthArticles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
