using Markdown.Tokens;

namespace Markdown;

public interface ILexer
{
    List<Token> Tokenize(string input);
}