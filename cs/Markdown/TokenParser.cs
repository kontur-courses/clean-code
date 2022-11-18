using Markdown.Abstractions;
using Markdown.Primitives;

namespace Markdown;

public class TokenParser : ITokenParser
{
    public IEnumerable<TagNode> Parse(IEnumerable<Token> tokens)
    {
        throw new NotImplementedException();
    }
}