namespace Markdown.Parsers
{
    public interface ITokenizer
    {
        (Token[] tokens, string newText) Tokenize (string text);
    }
}
