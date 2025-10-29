using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Yinyang_Api.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int SkillId { get; set; }
        public int RequestId { get; set; }
        [ForeignKey("SkillId")]
        public Skill Skill { get; set; }
        [ForeignKey("RequesterId")]
        public User Requester { get; set; }
        public string status { get; set; } = "pending"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
