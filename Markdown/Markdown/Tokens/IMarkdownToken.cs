using System.Collections.Generic;

namespace Markdown.Tokens
{
    public interface IMarkdownToken
    {
        public string Tag { get; }
        
        public IEnumerable<IMarkdownToken> Value { get; }

        public IHtmlToken ToHtmlToken();

        public string ToString();
    }
}