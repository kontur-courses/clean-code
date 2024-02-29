namespace MarkdownTask
{
    public interface IMarkdownParser
    {
        ICollection<Token> Parse(string markdown);
    }
}