using Markdown.Primitives;

namespace Markdown.Abstractions;

public interface ITokenizer
{
    IEnumerable<Token> Tokenize(string text);
}