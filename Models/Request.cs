   using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace Yinyang_Api.Models
    {
        public class Request
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int RequestId { get; set; }

            public int SkillId { get; set; }
            public Skill? Skill { get; set; }

            public int RequesterId { get; set; }
            public User? Requester { get; set; }

            public string Status { get; set; } = "Pending";
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    }
