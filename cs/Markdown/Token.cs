namespace Markdown
{
    public enum TokenName
    {
        Start,
        Header,
        NewLine,
        SingleUnderline,
        DoubleUnderline,
        Whitespace,
        Number,
        Word,
        Escape,
        Eof,
    }

    public class TagEvent
    {
        public TokenName Name;
        public string Content;

        public TagEvent(TokenName name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}
