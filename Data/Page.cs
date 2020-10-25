namespace broken_picturephone_blazor.Data
{
    public class Page
    {
        public Player Author { get; set; }
        public string Content { get; set; }
        public ContentType ContentType { get; set; }
        public int PageNumber { get; set; }
        public bool InProgress { get; set; }

        public Page()
        {
            Content = string.Empty;
            InProgress = true;
        }
    }
}