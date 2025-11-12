using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yinyang_Api.Data;
using Yinyang_Api.Dto;
using Yinyang_Api.Models;

namespace Yinyang_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] CreateMeassageDto messageDto)
        {
            if (messageDto == null || string.IsNullOrWhiteSpace(messageDto.Content))
                return BadRequest("Message content cannot be empty.");

            var sender = await _context.Users.FindAsync(messageDto.SenderId);
            var receiver = await _context.Users.FindAsync(messageDto.ReceiverId);

            if (sender == null || receiver == null)
                return NotFound("Invalid sender or receiver.");

            var message = new Message
            {
                SenderId = messageDto.SenderId,
                ReceiverId = messageDto.ReceiverId,
                Content = messageDto.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }


        [HttpGet("conversation/{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetConversation(int user1Id, int user2Id)
        {
            var messages = await _context.Messages
                .Where(m =>
                    (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                    (m.SenderId == user2Id && m.ReceiverId == user1Id))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            return Ok(messages);
        }

       
        [HttpGet("inbox/{userId}")]
        public async Task<IActionResult> GetInbox(int userId)
        {
            var messages = await _context.Messages
                .Where(m => m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return Ok(messages);
        }
    }
}
