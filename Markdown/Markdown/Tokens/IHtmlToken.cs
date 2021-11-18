using System.Collections.Generic;

namespace Markdown.Tokens
{
    public interface IHtmlToken
    {
        public string Tag { get; }
        
        public IEnumerable<IHtmlToken> Value { get; }

        public IMarkdownToken ToMarkdownToken();

        public string ToString();
    }
}