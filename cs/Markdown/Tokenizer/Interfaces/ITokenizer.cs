using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.Interfaces;

public interface ITokenizer
{
    IReadOnlyList<Token> Tokenize(string paragraphText);
}