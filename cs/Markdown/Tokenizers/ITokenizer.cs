namespace Markdown.Tokenizers;

public interface ITokenizer
{
    public List<Token> Tokenize(string markdown);
}