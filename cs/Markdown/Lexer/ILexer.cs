using Markdown.Tokens;

namespace Markdown.Lexer;

public interface ILexer
{
    IEnumerable<Token> Tokenize(string line);
}