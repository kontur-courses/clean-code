namespace Markdown
{
    public interface IMarkdownParser
    {
        MarkdownDocument Parse(string markdown);
    }
}