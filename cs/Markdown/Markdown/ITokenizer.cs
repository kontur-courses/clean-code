using Markdown.Markdown.Tokens;

namespace Markdown.Markdown;

public interface ITokenizer
{
    IList<IToken> Tokenize(string text);
}