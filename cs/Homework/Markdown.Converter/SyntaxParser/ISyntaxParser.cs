using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.SyntaxParser
{
    public interface ISyntaxParser
    {
        IEnumerable<TokenTree> Parse(IEnumerable<Token> lexedTokens);
    }
}