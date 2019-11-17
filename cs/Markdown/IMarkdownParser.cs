namespace Markdown
{
    public interface IMarkdownParser
    {
        RootToken Parse(string markdown);
    }
}