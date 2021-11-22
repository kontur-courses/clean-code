using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Markings
{
    public class MarkingTree<T> : IMarkingTree<T>
        where T : IToken
    {
        public T RootToken { get; }

        public MarkingTree(T rootToken)
        {
            RootToken = rootToken;
        }
    }
}