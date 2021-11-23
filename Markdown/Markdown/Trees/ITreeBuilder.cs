using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Trees
{
    public interface ITreeBuilder<T> where T : IToken
    {
        public IMarkingTree<T> Build(IEnumerable<T> tokens);
    }
}