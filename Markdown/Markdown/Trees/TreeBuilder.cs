using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Trees
{
    public class MarkdownTreeBuilder : ITreeBuilder<MarkdownToken>
    {
        public IMarkingTree<MarkdownToken> Build(IEnumerable<MarkdownToken> tokens)
        {
            throw new System.NotImplementedException();
        }
    }
}