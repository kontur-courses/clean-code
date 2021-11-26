using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface IParser
    {
        public List<Token> Parse(string text);
    }
}