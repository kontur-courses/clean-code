namespace MarkdownTask
{
    public interface ITagParser
    {
        ICollection<Token> Parse(string markdown);
    }
}