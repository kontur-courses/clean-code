namespace Markdown.Parsers
{
    public interface IParser
    {
        IToken TryGetToken();
    }
}
