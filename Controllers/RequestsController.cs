using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yinyang_Api.Data;
using Yinyang_Api.Models;
using Yinyang_Api.Dto;

namespace Yinyang_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDetailDto>> GetRequest(int id)
        {
            var request = await _context.Requests
                .Include(r => r.Skill)
                    .ThenInclude(s => s.Owner)
                .Include(r => r.Requester)
                .FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null)
                return NotFound();

            var requestDto = new RequestDetailDto
            {
                RequestId = request.RequestId,
                SkillId = request.SkillId,
                RequesterId = request.RequesterId,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                Skill = request.Skill == null ? null : new SkillDto
                {
                    Id = request.Skill.Id,
                    Title = request.Skill.Title,
                    Category = request.Skill.Category,
                    Location = request.Skill.Location,
                    Status = request.Skill.Status,
                    OwnerName = request.Skill.Owner?.Name ?? "Unknown"
                },
                Requester = request.Requester == null ? null : new UserDto
                {
                    Id = request.Requester.Id,
                    Name = request.Requester.Name,
                    Email = request.Requester.Email,
                    Location = request.Requester.Location
                }
            };

            return Ok(requestDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] RequestDto updatedRequest)
        {
            var existingRequest = await _context.Requests.FindAsync(id);

            if (existingRequest == null)
                return NotFound();

            existingRequest.Status = updatedRequest.Status ?? existingRequest.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
                return NotFound();

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] RequestDto requestDto)
        {
            if (requestDto == null)
                return BadRequest("Invalid request data.");

            var skill = await _context.Skills.FindAsync(requestDto.SkillId);
            var requester = await _context.Users.FindAsync(requestDto.RequesterId);

            if (skill == null)
                return BadRequest("Skill not found.");
            if (requester == null)
                return BadRequest("Requester not found.");

            var request = new Request
            {
                SkillId = requestDto.SkillId,
                RequesterId = requestDto.RequesterId,
                Skill = skill,
                Requester = requester,
                Status = requestDto.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRequest), new { id = request.RequestId }, new
            {
                RequestId = request.RequestId,
                Message = "Request created successfully"
            });
        }

        // Additional endpoint to get all requests for a user
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<RequestDetailDto>>> GetUserRequests(int userId)
        {
            var requests = await _context.Requests
                .Include(r => r.Skill)
                    .ThenInclude(s => s.Owner)
                .Include(r => r.Requester)
                .Where(r => r.RequesterId == userId)
                .ToListAsync();

            var requestDtos = requests.Select(r => new RequestDetailDto
            {
                RequestId = r.RequestId,
                SkillId = r.SkillId,
                RequesterId = r.RequesterId,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                Skill = r.Skill == null ? null : new SkillDto
                {
                    Id = r.Skill.Id,
                    Title = r.Skill.Title,
                    Category = r.Skill.Category,
                    Location = r.Skill.Location,
                    Status = r.Skill.Status,
                    OwnerName = r.Skill.Owner?.Name ?? "Unknown"
                },
                Requester = r.Requester == null ? null : new UserDto
                {
                    Id = r.Requester.Id,
                    Name = r.Requester.Name,
                    Email = r.Requester.Email,
                    Location = r.Requester.Location
                }
            }).ToList();

            return Ok(requestDtos);
        }
    }
}