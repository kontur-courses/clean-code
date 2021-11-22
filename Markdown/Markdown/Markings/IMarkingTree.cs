using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Markings
{
    public interface IMarkingTree<out T>
        where T : IToken
    {
        public T RootToken { get; }
    }
}