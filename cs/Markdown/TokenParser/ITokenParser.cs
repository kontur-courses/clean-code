using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public interface ITokenParser
    {
        IEnumerable<TagNode> Parse(IEnumerable<Token> tokens);
    }
}