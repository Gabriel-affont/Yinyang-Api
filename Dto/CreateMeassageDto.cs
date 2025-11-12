namespace Yinyang_Api.Dto
{
    public class CreateMeassageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
