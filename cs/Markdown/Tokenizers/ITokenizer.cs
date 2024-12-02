using Markdown.Tokens;

namespace Markdown.Tokenizers;

public interface ITokenizer
{
    public List<Token> SplitToTokens(string text);
}