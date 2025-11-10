
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yinyang_Api.Data;
using Yinyang_Api.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


using Yinyang_Api.Dto;
using System.IdentityModel.Tokens.Jwt;
namespace Yinyang_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SkillsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkills(int id)
        {
            var skills = await _context.Skills
                .Include(s => s.Owner)
                .Select(s => new SkillDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Category = s.Category,
                    Location = s.Location,
                    Status = s.Status,
                    OwnerName = s.Owner.Name
                })
                .ToListAsync();

            return Ok(skills);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetSkill(int id)
        {
            var skill = await _context.Skills
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (skill == null)
                return NotFound();


            return Ok(skill);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<SkillDto>> CreateSkill(CreateSkillDto createSkillDto) 
        {
            if (createSkillDto == null)
                return BadRequest("Skill data is required");

            try
            {
                var userIdClaim = User.FindFirst("id")?.Value
                               ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("User ID not found in token");
                }

                User user = null;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    user = await _context.Users.FindAsync(userId);
                }
                else
                {
                    user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userIdClaim);
                }

                if (user == null)
                    return Unauthorized("User not found");

                var skill = new Skill
                {
                    Title = createSkillDto.Title,
                    Description = createSkillDto.Description,
                    Category = createSkillDto.Category ?? "",
                    Location = createSkillDto.Location ?? "",
                    Status = "available",
                    OwnerId = user.Id,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();

                
                var createdSkill = await _context.Skills
                    .Include(s => s.Owner)
                    .FirstOrDefaultAsync(s => s.Id == skill.Id);

                var skillDto = new SkillDto
                {
                    Id = createdSkill.Id,
                    Title = createdSkill.Title,
                    Category = createdSkill.Category,
                    Location = createdSkill.Location,
                    Status = createdSkill.Status,
                    OwnerName = createdSkill.Owner.Name
                };

                return CreatedAtAction(nameof(GetSkill), new { id = skillDto.Id }, skillDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkill(int id, Skill updatedSkill)
        {
            if (id != updatedSkill.Id)
                return BadRequest();

            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound();

            skill.Title = updatedSkill.Title;
            skill.Description = updatedSkill.Description;
            skill.Category = updatedSkill.Category;
            skill.Location = updatedSkill.Location;
            skill.Status = updatedSkill.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(int id)        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound();
            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
