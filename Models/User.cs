using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Yinyang_Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public ICollection<Request> RequestMade { get; set; } = new List<Request>();
    }
}
