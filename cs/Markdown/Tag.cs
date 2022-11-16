namespace Markdown
{
    public class Tag
    {
        public string OpenTag { get; }
        public string CloseTag { get; }

        public Tag(string open, string close)
        {
            OpenTag = open;
            CloseTag = close;
        }
    }
}