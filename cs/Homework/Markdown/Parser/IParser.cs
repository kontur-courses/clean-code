using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface IParser<TToken>
    {
        public IEnumerable<TToken> Parse(string text);
    }
}