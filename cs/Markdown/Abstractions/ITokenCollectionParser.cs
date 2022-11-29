using Markdown.Primitives;

namespace Markdown.Abstractions;

public interface ITokenCollectionParser
{
    IEnumerable<TagNode> Parse(IEnumerable<Token> tokens);
}