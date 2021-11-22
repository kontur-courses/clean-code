using System.Collections.Generic;
using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Factories
{
    public interface IMarkingTreeFactory<T>
        where T : IToken
    {
        public IMarkingTree<T> NewMarking(T rootToken);
    }
}