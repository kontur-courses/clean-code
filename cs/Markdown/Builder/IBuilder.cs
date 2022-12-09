using Markdown.TokenNamespace;
using System.Collections.Generic;

namespace Markdown.BuilderNamespace
{
    public interface IBuilder
    {
        public string Build(IEnumerable<IToken> tokens);
    }
}
