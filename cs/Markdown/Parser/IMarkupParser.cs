using System.Collections.Generic;

namespace Markdown.Parser
{
    public interface IMarkupParser
    {
        public List<TextToken> Parse(string text);
    }
}