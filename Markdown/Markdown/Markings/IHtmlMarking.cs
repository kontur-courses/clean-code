using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Markings
{
    public interface IHtmlMarking
    {
        public IEnumerable<IHtmlToken> Tokens { get; }

        public IMarkdownMarking ToMarkdownMarking();

        public string ToString();
    }
}