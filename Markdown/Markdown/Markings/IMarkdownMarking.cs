using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Markings
{
    public interface IMarkdownMarking
    {
        public IEnumerable<IMarkdownToken> Tokens { get; }

        public string ToString();
    }
}