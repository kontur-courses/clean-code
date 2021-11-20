using System.Collections.Generic;
using Markdown.SyntaxParser;

namespace Markdown.Parser
{
    public interface IParser
    {
        IEnumerable<TokenTree> Parse(string text);
    }
}