using System.Collections.Generic;
using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Factories
{
    public class MarkdownMarkingFactory : IMarkingFactory<MarkdownToken, MarkdownMarking>
    {
        public MarkdownMarking NewMarking(IEnumerable<IEnumerable<MarkdownToken>> tokensLines)
        {
            return new MarkdownMarking(tokensLines);
        }
    }
}