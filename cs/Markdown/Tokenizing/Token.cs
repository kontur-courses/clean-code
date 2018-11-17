namespace Markdown.Tokenizing
{
    public class Token
    {
        public Tag Tag;
        public string Content;
        public bool IsOpening;

        public Token(Tag tag, bool isOpening, string content = null)
        {
            Tag = tag;
            IsOpening = isOpening;
            Content = content;
        }
    }
}
