using Markdown.Primitives;

namespace Markdown.Abstractions;

public interface ITokenParser
{
    IEnumerable<TagNode> Parse(IEnumerable<Token> tokens);
}