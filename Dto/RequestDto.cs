namespace Yinyang_Api.Dto
{
    public class RequestDto
    {
        public int RequestId { get; set; }
        public int SkillId { get; set; }
        public int RequesterId { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
