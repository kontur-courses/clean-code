using Markdown.Tokens;

namespace Markdown.Lexer;

public interface ILexer
{
    List<Token> Tokenize(string line);
}