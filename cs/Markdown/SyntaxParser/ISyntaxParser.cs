using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.SyntaxParser
{
    public interface ISyntaxParser
    {
        IEnumerable<Token> Parse(IEnumerable<Token> tokens);
    }
}