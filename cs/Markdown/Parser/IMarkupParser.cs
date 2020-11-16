using System.Collections.Generic;

namespace Markdown.Parser
{
    public interface IMarkupParser
    {
        public TextData Parse(string text);
    }
}