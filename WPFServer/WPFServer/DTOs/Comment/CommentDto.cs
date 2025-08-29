namespace WPFServer.DTOs.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Text { get; set; }
        public string? Date { get; set; }
        public bool? IsPerson { get; set; }
    }
}
