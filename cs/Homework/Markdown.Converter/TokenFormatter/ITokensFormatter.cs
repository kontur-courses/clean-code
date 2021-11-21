using System.Collections.Generic;
using Markdown.SyntaxParser;

namespace Markdown.TokenFormatter
{
    public interface ITokensFormatter
    {
        string Format(IEnumerable<TokenTree> tokens);
    }
}