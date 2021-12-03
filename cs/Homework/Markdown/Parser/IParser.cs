using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface IParser<TToken>
    {
        public List<TToken> Parse(string text);
    }
}