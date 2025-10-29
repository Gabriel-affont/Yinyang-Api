using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Yinyang_Api.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string Category { get; set; }
        public  string Location{ get; set; }
        public required string Status { get; set; } = "available";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        public ICollection<Request> Requests { get; set; } = new List<Request>();

    }
}
