using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yinyang_Api.Data;
using Yinyang_Api.Dto;
using Yinyang_Api.Models;

namespace Yinyang_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto dto)
        {
            var skill = await _context.Skills.FindAsync(dto.SkillId);
            var user = await _context.Users.FindAsync(dto.UserId);

            if (skill == null || user == null)
                return NotFound("Skill or User not found");

            var review = new Review
            {
                SkillId = dto.SkillId,
                UserId = dto.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }



        [HttpGet("skill/{skillId}")]
        public async Task<IActionResult> GetReviewsForSkill(int skillId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.SkillId == skillId)
                .Include(r => r.User)
                .ToListAsync();

            return Ok(reviews);
        }


        [HttpGet("average/{skillId}")]
        public async Task<IActionResult> GetAverageRating(int skillId)
        {
            var avg = await _context.Reviews
                .Where(r => r.SkillId == skillId)
                .AverageAsync(r => (double?)r.Rating) ?? 0.0;

            return Ok(new { skillId, averageRating = Math.Round(avg, 1) });
        }

    }


}
