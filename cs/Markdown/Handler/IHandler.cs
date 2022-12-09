using Markdown.TokenNamespace;
using System.Collections.Generic;

namespace Markdown.HandlerNamespace
{
    public interface IHandler
    {
        public IEnumerable<IToken> Handle(IEnumerable<IToken> tokens);
    }
}
