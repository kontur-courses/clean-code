namespace Markdown.TagParsers
{
    public enum State
    {
        Start,
        FindClosingUnderline,
        CheckClosingContext,
        Whitespace,
        Header,
        NewLine,
        Other
    }
}
