namespace WPFServer.DTOs.Comment
{
    public class LiteCommentDto
    {
        public int Id { get; set; }
        public string PersonId { get; set; } = null!;
        public string Text { get; set; } = null!;
    }
}
