namespace Yinyang_Api.Dto
{
    public class CreateSkillDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string Category { get; set; } = "";
        public string Location { get; set; } = "";
    }
}
