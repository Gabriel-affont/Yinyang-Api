
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yinyang_Api.Data;
using Yinyang_Api.Models;

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
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            var skills = await _context.Skills
                  .Include(s => s.Owner)
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
        [HttpPost]
        public async Task<ActionResult<Skill>> CreateSkill(Skill skill)
        {
            if (skill == null)
                return BadRequest();
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSkill), new { id = skill.Id }, skill);
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
