using System.Collections.Generic;
using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Factories
{
    public class MarkingTreeFactory<T> : IMarkingTreeFactory<T>
        where T : IToken
    {
        public IMarkingTree<T> NewMarking(T rootToken)
        {
            return new MarkingTree<T>(rootToken);
        }
    }
}