namespace WPFServer.DTOs.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string Date { get; set; } = null!;
        public bool IsPerson { get; set; }
    }
}
