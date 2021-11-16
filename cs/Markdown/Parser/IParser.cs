using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface IParser
    {
        IEnumerable<Token> Parse(string text);
    }
}