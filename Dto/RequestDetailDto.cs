namespace Yinyang_Api.Dto
{
    public class RequestDetailDto
    {
        internal UserDto? Requester;

        public int RequestId { get; set; }
        public int SkillId { get; set; }
        public SkillDto Skill { get; set; }
        public int RequesterId { get; set; }
        
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
